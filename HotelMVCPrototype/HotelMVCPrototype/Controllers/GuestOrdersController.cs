using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GuestOrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public GuestOrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Show menu for a specific room
    public async Task<IActionResult> Index(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null)
            return NotFound();

        ViewBag.RoomNumber = room.Number; // THIS IS 30
        ViewBag.RoomId = room.Id;         // THIS IS 3, THE DB ID YOU NEED

        var menu = await _context.MenuItems.ToListAsync();
        return View(menu);
    }


    [HttpPost]
    public async Task<IActionResult> PlaceOrder(int roomId, List<int> menuItemIds, List<int> quantities)
    {
        if (menuItemIds == null || menuItemIds.Count == 0)
            return BadRequest("No items selected.");

        var order = new Order
        {
            RoomId =  roomId,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.New,
            Items = new List<OrderItem>()
        };

        for (int i = 0; i < menuItemIds.Count; i++)
        {
            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItemIds[i],
                Quantity = quantities[i]
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Load order with items for summary
        var orderWithItems = await _context.Orders
        .Include(o => o.Room)              // include Room!
        .Include(o => o.Items)
        .ThenInclude(i => i.MenuItem)
        .FirstOrDefaultAsync(o => o.Id == order.Id);


        return View("ThankYou", orderWithItems);
    }

    // Thank you page now receives the order to display summary
    public IActionResult ThankYou(Order order) => View(order);
}
