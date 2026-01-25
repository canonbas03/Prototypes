using HotelMVCPrototype.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")] // or Admin
public class AuditLogsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuditLogsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(
        string? user,
        string? auditAction,
        string? entityType,
        int? entityId,
        int days = 7)
    {
        var from = DateTime.Now.AddDays(-days);

        var q = _context.AuditLogs
            .Where(l => l.CreatedAt >= from);

        if (!string.IsNullOrWhiteSpace(user))
            q = q.Where(l => l.UserName!.Contains(user));

        if (!string.IsNullOrWhiteSpace(auditAction))
            q = q.Where(l => l.Action == auditAction);

        if (!string.IsNullOrWhiteSpace(entityType))
            q = q.Where(l => l.EntityType == entityType);

        if (entityId.HasValue)
            q = q.Where(l => l.EntityId == entityId);

        var logs = await q
            .OrderByDescending(l => l.CreatedAt)
            .Take(500)
            .ToListAsync();

        ViewBag.Days = days;
        return View(logs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var log = await _context.AuditLogs.FirstOrDefaultAsync(x => x.Id == id);
        if (log == null) return NotFound();
        return View(log);
    }

}
