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
        public async Task<IActionResult> Index(int floor = 1)
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

            var roomMap = rooms
                .Where(r => r.Floor == floor)
                .Select(r => new RoomMapViewModel
                {
                    RoomId = r.Id,
                    Number = r.Number,
                    Top = r.MapTop,
                    Left = r.MapLeft,
                    Width = r.MapWidth,
                    Height = r.MapHeight,
                    StatusColor = r.Status switch
                    {
                        RoomStatus.Available => "#164E63",   // Forest Green
                        RoomStatus.Occupied => "#86A789",    // Sage
                        RoomStatus.Cleaning => "#e8c53a",    // Soft Mint
                        RoomStatus.Maintenance => "#DC3545", // Red
                        _ => "#FFFFFF"
                    }
                })
                .ToList();


            var vm = new ReceptionDashboardViewModel
            {
                Rooms = rooms,
                RoomStatistics = await _statsService.GetStatisticsAsync(),
                NewRequests = requests,
                RoomMap = roomMap,
                CurrentFloor = floor
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

        // GET: Reception/RoomDetails/5
        public async Task<IActionResult> RoomDetails(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.GuestAssignments
                    .Where(g => g.IsActive))
                .ThenInclude(ga => ga.Guests)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return NotFound();

            return View(room);
        }

        


    }
}
