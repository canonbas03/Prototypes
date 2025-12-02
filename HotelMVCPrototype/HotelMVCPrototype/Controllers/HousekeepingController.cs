using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Housekeeping")]
    public class HousekeepingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HousekeepingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Housekeeping
        public async Task<IActionResult> Index()
        {
            var cleaningRooms = await _context.Rooms
                .Where(r => r.Status == RoomStatus.Cleaning)
                .ToListAsync();

            return View(cleaningRooms);
        }

        // POST: Mark Cleaned
        [HttpPost]
        public async Task<IActionResult> MarkCleaned(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            if (room.Status != RoomStatus.Cleaning)
            {
                return BadRequest("Room is not in cleaning state.");
            }

            room.Status = RoomStatus.Available;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
