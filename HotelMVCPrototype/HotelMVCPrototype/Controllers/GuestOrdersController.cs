using Azure.Core;
using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class GuestOrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<BarHub> _hubContext;
    private const string CART_KEY = "GuestCart";
    public GuestOrdersController(
        ApplicationDbContext context,
        IHubContext<BarHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<IActionResult> Index(int roomId)
    {
        var menuItems = await _context.MenuItems
            .Where(m => m.IsActive)
            .OrderBy(m => m.Category)
            .ToListAsync();


        var cart = HttpContext.Session
        .GetObject<Dictionary<int, int>>(CART_KEY)
        ?? new Dictionary<int, int>();

        ViewBag.Cart = cart;
        ViewBag.RoomId = roomId;

        return View(menuItems);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(int roomId, Dictionary<int, int> items)
    {
        if (items == null || !items.Any())
            return RedirectToAction(nameof(Index), new { roomId });

        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == roomId);
        if (!roomExists)
        {
            return BadRequest("Invalid room.");
        }

        var cart = HttpContext.Session
                .GetObject<Dictionary<int, int>>(CART_KEY)
                ?? new Dictionary<int, int>();

        ViewBag.Cart = cart;

        var order = new Order
        {
            RoomId = roomId,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.New,
            Items = new List<OrderItem>()
        };

        foreach (var (menuItemId, qty) in items)
        {
            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItemId,
                Quantity = qty
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("ReceiveNewOrder", order.Id);

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

    public async Task<IActionResult> Checkout(int roomId)
    {
        

        var cart = HttpContext.Session
            .GetObject<Dictionary<int, int>>(CART_KEY);

        if (cart == null || !cart.Any())
            return RedirectToAction(nameof(Index), new { roomId });

        var menuItems = await _context.MenuItems
            .Where(m => cart.Keys.Contains(m.Id))
            .ToListAsync();

        var model = menuItems.Select(m => new CheckoutItemViewModel
        {
            MenuItemId = m.Id,
            Name = m.Name,
            Price = m.Price,
            Quantity = cart[m.Id]
        }).ToList();

        ViewBag.RoomId = roomId;

        return View(model);
    }

    [HttpPost]
    public IActionResult UpdateCart([FromBody] Dictionary<int, int> cart)
    {
        HttpContext.Session.SetObject(CART_KEY, cart);
        return Ok();
    }


}
