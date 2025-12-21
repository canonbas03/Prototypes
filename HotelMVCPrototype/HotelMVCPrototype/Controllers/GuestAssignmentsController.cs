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
            //var assignments = await _context.GuestAssignments
            //                        .Include(g => g.Room)
            //                        .ToListAsync();
            return View();
        }

        // GET: GuestAssignments/Create

        // GET: GuestAssignments/Create
        public IActionResult Create(int roomId)
        {
            var model = new CreateStayViewModel
            {
                RoomId = roomId,
                CheckInDate = DateTime.Today,
                Guests = new List<GuestInputViewModel>
                    {
                        new GuestInputViewModel(), // at least 1 guest
                    }
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStayViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var room = await _context.Rooms.FindAsync(model.RoomId);
            if (room == null)
                return NotFound();

            var stay = new GuestAssignment
            {
                RoomId = model.RoomId,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                IsActive = true,
                Guests = new List<Guest>()
            };

            foreach (var g in model.Guests)
            {
                stay.Guests.Add(new Guest
                {
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    EGN = g.EGN,
                    BirthDate = g.BirthDate,
                    Sex = g.Sex,
                    Nationality = g.Nationality,
                    Phone = g.Phone
                });
            }

            room.Status = RoomStatus.Occupied;

            _context.GuestAssignments.Add(stay);
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("RoomDetails", "Reception", new { id = model.RoomId });
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
