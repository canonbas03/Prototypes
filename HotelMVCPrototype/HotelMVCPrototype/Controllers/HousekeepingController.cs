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
        [Authorize(Roles = "Housekeeping")]
        public async Task<IActionResult> Index(int floor = 1)
        {
            var cleaningRooms = await _context.Rooms
    .Where(r => r.Status == RoomStatus.Cleaning && r.Floor == floor)
    .ToListAsync();

            var roomMap = cleaningRooms
                .Select(r => new RoomMapViewModel
                {
                    RoomId = r.Id,
                    Number = r.Number,
                    TopPercent = r.MapTopPercent,
                    LeftPercent = r.MapLeftPercent,
                    WidthPercent = r.MapWidthPercent,
                    HeightPercent = r.MapHeightPercent,
                    StatusColor = "#0dcaf0"
                })
                .ToList();

            var vm = new HousekeepingDashboardViewModel
            {
                Rooms = cleaningRooms,
                RoomStatistics = await _statsService.GetStatisticsAsync(),
                RoomMapPage = new RoomMapPageViewModel
                {
                    CurrentFloor = floor,
                    Mode = RoomMapMode.Housekeeping,
                    Rooms = roomMap
                }
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
