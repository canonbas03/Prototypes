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

        public async Task<IActionResult> Index()
        {
            var issues = await _context.RoomIssues
                .Include(i => i.Room)
                .Where(i => i.Category == IssueCategory.Maintenance &&
                            i.Status != IssueStatus.Resolved)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(issues);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acknowledge(int id)
        {
            var issue = await _context.RoomIssues.FindAsync(id);
            if (issue == null) return NotFound();
            if (issue.Category != IssueCategory.Maintenance) return BadRequest();

            issue.Status = IssueStatus.InProgress;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(int id)
        {
            var issue = await _context.RoomIssues.FindAsync(id);
            if (issue == null) return NotFound();
            if (issue.Category != IssueCategory.Maintenance) return BadRequest();

            issue.Status = IssueStatus.Resolved;
            issue.ResolvedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Optional: keep MarkDone for compatibility (calls Resolve)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> MarkDone(int issueId) => Resolve(issueId);
    }
}
