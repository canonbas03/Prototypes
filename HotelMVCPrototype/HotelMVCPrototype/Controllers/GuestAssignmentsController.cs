using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    public class GuestAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GuestAssignments
        public async Task<IActionResult> Index()
        {
            var assignments = await _context.GuestAssignments
                                    .Include(g => g.Room)
                                    .ToListAsync();
            return View(assignments);
        }

        // GET: GuestAssignments/Create

        // GET: GuestAssignments/Create
        public async Task<IActionResult> Create()
        {
            var rooms = await _context.Rooms
                            .Where(r => r.Status == Models.Enums.RoomStatus.Available)
                            .ToListAsync();

            // Create a SelectList for the dropdown
            ViewBag.Rooms = new SelectList(rooms, "Id", "Number");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestAssignment assignment)
        {
            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(assignment.RoomId);
                if (room == null || room.Status != RoomStatus.Available)
                {
                    ModelState.AddModelError("RoomId", "Selected room is not available.");
                }
                else
                {
                    room.Status = RoomStatus.Occupied;
                    assignment.Room = room;

                    _context.GuestAssignments.Add(assignment);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload dropdown with selected value preserved
            ViewBag.Rooms = new SelectList(
                await _context.Rooms.Where(r => r.Status == RoomStatus.Available).ToListAsync(),
                "Id",
                "Number",
                assignment.RoomId
            );

            return View(assignment);
        }


        // GET: GuestAssignments/CheckOut/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null) return NotFound();

            var assignment = await _context.GuestAssignments
                                .Include(g => g.Room)
                                .FirstOrDefaultAsync(g => g.Id == id);

            if (assignment == null) return NotFound();

            assignment.CheckOutDate = DateTime.Now;
            assignment.IsActive = false;

            assignment.Room.Status = Models.Enums.RoomStatus.Cleaning;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
