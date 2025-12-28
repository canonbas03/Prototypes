using HotelMVCPrototype.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GuestRoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public GuestRoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int roomId)
    {
        var room = await _context.Rooms
            .Include(r => r.GuestAssignments)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null)
            return NotFound();

        return View(room);
    }

    // QR link landing page
    public async Task<IActionResult> Room(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) return NotFound();

        return View(room);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleDnd(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) return NotFound();

        room.IsDND = !room.IsDND;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Room), new { roomId });
    }
}
