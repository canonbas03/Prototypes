using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Reception, Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (await _context.Rooms.AnyAsync(r => r.Number == room.Number))
            {
                ModelState.AddModelError("Number", "Room number already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }


        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room editedRoom)
        {
            if (id != editedRoom.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(editedRoom);

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            // Update ONLY editable fields
            room.Number = editedRoom.Number;
            room.Status = editedRoom.Status;
            room.Type = editedRoom.Type;

            // ⚠️ Keep mapping fields untouched
            // room.Floor stays
            // room.MapTop stays
            // room.MapLeft stays
            // room.MapWidth stays
            // room.MapHeight stays

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null) return NotFound();
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
