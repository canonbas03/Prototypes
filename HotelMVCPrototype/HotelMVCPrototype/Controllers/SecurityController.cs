using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Security")]
public class SecurityController : Controller
{
    private readonly ApplicationDbContext _context;

    public SecurityController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var incidents = await _context.SecurityIncidents
            .Include(i => i.Room)
            .OrderBy(i => i.Status)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync();

        return View(incidents);
    }

    [HttpPost]
    public async Task<IActionResult> Acknowledge(int id)
    {
        var incident = await _context.SecurityIncidents.FindAsync(id);
        if (incident == null) return NotFound();

        incident.Status = SecurityIncidentStatus.Acknowledged;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Resolve(int id)
    {
        var incident = await _context.SecurityIncidents.FindAsync(id);
        if (incident == null) return NotFound();

        incident.Status = SecurityIncidentStatus.Resolved;
        incident.ResolvedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
