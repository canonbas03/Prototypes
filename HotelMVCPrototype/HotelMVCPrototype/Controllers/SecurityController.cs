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
        var issues = await _context.RoomIssues
           .Include(i => i.Room)
           .Where(i => i.Category == IssueCategory.Security && i.Status != IssueStatus.Resolved)
           .OrderByDescending(i => i.CreatedAt)
           .ToListAsync();

        return View(issues);
    }

    [HttpPost]
    public async Task<IActionResult> Acknowledge(int id)
    {
        var issue = await _context.RoomIssues.FindAsync(id);
        if (issue == null) return NotFound();
        if (issue.Category != IssueCategory.Security) return BadRequest();

        // "Acknowledged" == InProgress
        issue.Status = IssueStatus.InProgress;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Resolve(int id)
    {
        var issue = await _context.RoomIssues.FindAsync(id);
        if (issue == null) return NotFound();
        if (issue.Category != IssueCategory.Security) return BadRequest();

        issue.Status = IssueStatus.Resolved;
        issue.ResolvedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
