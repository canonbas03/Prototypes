using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using HotelMVCPrototype.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Reception")]
    public class ReceptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoomStatisticsService _statsService;

        public ReceptionController(ApplicationDbContext context, IRoomStatisticsService statsService)
        {
            _context = context;
            _statsService = statsService;
        }


        // GET: Reception Dashboard
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.GuestAssignments.Where(g => !g.IsActive))
                .ToListAsync();

            var requests = await _context.ServiceRequests
                .Include(r => r.Room)
                .Include(r => r.Items)
                .Where(r => r.Status == ServiceRequestStatus.New)
                .OrderBy(r => r.CreatedAt)
                .Take(3) // side window, not full list
                .ToListAsync();

            var vm = new ReceptionDashboardViewModel
            {
                Rooms = rooms,
                RoomStatistics = await _statsService.GetStatisticsAsync(),
                NewRequests = requests
            };

            return View(vm);
        }

        // Quick Check-In (GET)
        public async Task<IActionResult> CheckIn(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null || room.Status != RoomStatus.Available)
                return BadRequest("Room is not available.");

            var assignment = new GuestAssignment
            {
                RoomId = room.Id,
                CheckInDate = DateTime.Now
            };

            return View(assignment);
        }

        // Quick Check-In (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(GuestAssignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return View(assignment);
            }

            var room = await _context.Rooms.FindAsync(assignment.RoomId);
            if (room == null || room.Status != RoomStatus.Available)
            {
                ModelState.AddModelError("", "Room is not available.");
                return View(assignment);
            }

            room.Status = RoomStatus.Occupied;
            assignment.Room = room;
            _context.GuestAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Optional: Check-Out action (you already have this)
    }
}
