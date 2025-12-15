using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Bar")]
    public class BarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .Include(o => o.Room)
            .Where(o => o.Status == OrderStatus.New || o.Status == OrderStatus.InProgress)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();



            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
