using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Reception")]
public class ReceptionRequestsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReceptionRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _context.ServiceRequests
            .Include(r => r.Room)
            .Include(r => r.Items)
            .Where(r => r.Status == ServiceRequestStatus.New)
            .OrderBy(r => r.CreatedAt) // queue
            .ToListAsync();

        return View(requests);
    }

    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var request = await _context.ServiceRequests.FindAsync(id);
        if (request == null) return NotFound();

        request.Status = ServiceRequestStatus.Completed;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
