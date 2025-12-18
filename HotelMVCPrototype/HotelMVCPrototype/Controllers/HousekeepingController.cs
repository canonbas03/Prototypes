using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using HotelMVCPrototype.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Housekeeping")]
    public class HousekeepingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoomStatisticsService _statsService;

        public HousekeepingController(ApplicationDbContext context, IRoomStatisticsService statsService)
        {
            _context = context;
            _statsService = statsService;
        }


        // GET: Housekeeping
        public async Task<IActionResult> Index()
        {
            var cleaningRooms = await _context.Rooms
                .Where(r => r.Status == RoomStatus.Cleaning)
                .ToListAsync();

            var vm = new HousekeepingDashboardViewModel
            {
                Rooms = cleaningRooms,
                RoomStatistics = await _statsService.GetStatisticsAsync()
            };

            return View(vm);
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
