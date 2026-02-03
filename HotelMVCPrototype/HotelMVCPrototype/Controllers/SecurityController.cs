using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using HotelMVCPrototype.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Security")]
public class SecurityController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditLogger _audit;

    public SecurityController(ApplicationDbContext context, IAuditLogger audit)
    {
        _context = context;
        _audit = audit;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Acknowledge(int id)
    {
        var issue = await _context.RoomIssues
            .Include(i => i.Room)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null) return NotFound();
        if (issue.Category != IssueCategory.Security) return BadRequest();

        issue.Status = IssueStatus.InProgress;
        await _context.SaveChangesAsync();

        var roomLabel = issue.Room != null ? $"Room {issue.Room.Number}" : "No room";

        await _audit.LogAsync(
            action: "SecurityAcknowledged",
            entityType: "RoomIssue",
            entityId: issue.Id,
            description: $"Security acknowledged: [{issue.TypeKey}] ({roomLabel})",
            data: new
            {
                issue.Id,
                issue.RoomId,
                RoomNumber = issue.Room?.Number,
                issue.TypeKey,
                issue.Description,
                NewStatus = issue.Status.ToString()
            }
        );

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Resolve(int id)
    {
        var issue = await _context.RoomIssues
            .Include(i => i.Room)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null) return NotFound();
        if (issue.Category != IssueCategory.Security) return BadRequest();

        issue.Status = IssueStatus.Resolved;
        issue.ResolvedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        var roomLabel = issue.Room != null ? $"Room {issue.Room.Number}" : "No room";

        await _audit.LogAsync(
            action: "SecurityResolved",
            entityType: "RoomIssue",
            entityId: issue.Id,
            description: $"Security resolved: [{issue.TypeKey}] ({roomLabel})",
            data: new
            {
                issue.Id,
                issue.RoomId,
                RoomNumber = issue.Room?.Number,
                issue.TypeKey,
                issue.ResolvedAt,
                issue.Description
            }
        );

        return RedirectToAction(nameof(Index));
    }
}
