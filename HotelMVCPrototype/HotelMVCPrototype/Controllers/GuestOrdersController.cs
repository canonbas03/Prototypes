using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class GuestOrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<BarHub> _hubContext;

    public GuestOrdersController(
        ApplicationDbContext context,
        IHubContext<BarHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // Show menu for a specific room
    public async Task<IActionResult> Index(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null)
            return NotFound();

        ViewBag.RoomNumber = room.Number;
        ViewBag.RoomId = room.Id;

        var menu = await _context.MenuItems.ToListAsync();
        return View(menu);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(int roomId, Dictionary<int, int> items)
    {
        if (items == null || !items.Any(i => i.Value > 0))
            return BadRequest("No items selected.");

        var order = new Order
        {
            RoomId = roomId,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.New,
            Items = new List<OrderItem>()
        };

        foreach (var item in items.Where(i => i.Value > 0))
        {
            order.Items.Add(new OrderItem
            {
                MenuItemId = item.Key,
                Quantity = item.Value
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Notify reception
        await _hubContext.Clients.All.SendAsync("ReceiveNewOrder", order.Id);

        // ✅ Redirect instead of returning View
        return RedirectToAction(nameof(ThankYou), new { id = order.Id });
    }

    [HttpGet]
    public async Task<IActionResult> ThankYou(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Include(o => o.Room)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        return View(order);
    }
}
