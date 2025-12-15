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

    // Show menu and place order
    public async Task<IActionResult> Index()
    {
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
            RoomId = roomId,
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

        return RedirectToAction(nameof(ThankYou));
    }

    public IActionResult ThankYou() => View();
}
