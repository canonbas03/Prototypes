using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVCPrototype.Controllers
{
    [Authorize(Roles = "Maintenance")]
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Maintenance
        public async Task<IActionResult> Index()
        {
            // Show only rooms needing maintenance
            var issues = await _context.RoomIssues
        .Include(i => i.Room)
        .Where(i =>
            i.Category == IssueCategory.Maintenance &&
            i.Status != IssueStatus.Resolved)
        .OrderByDescending(i => i.CreatedAt)
        .ToListAsync();

            return View(issues);
        }

        // POST: Mark Maintenance Done
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> MarkDone(int issueId)
        {
            var issue = await _context.RoomIssues.FindAsync(issueId);
            if (issue == null) return NotFound();

            issue.Status = IssueStatus.Resolved;
            issue.ResolvedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
