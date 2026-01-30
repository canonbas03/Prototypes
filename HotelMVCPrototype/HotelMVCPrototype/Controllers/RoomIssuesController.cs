using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


public class RoomIssuesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RoomIssuesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(IssueCategory category, int? roomId)
    {
        var vm = new CreateRoomIssueViewModel
        {
            Category = category,
            RoomId = roomId
        };

        if (roomId.HasValue)
        {
            vm.RoomNumber = await _context.Rooms
                .Where(r => r.Id == roomId.Value)
                .Select(r => (int?)r.Number)
                .FirstOrDefaultAsync();
        }

        // Only show room dropdown if roomId is missing
        if (!roomId.HasValue)
        {
            ViewBag.Rooms = await _context.Rooms
                .OrderBy(r => r.Number)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"Room {r.Number}"
                })
                .ToListAsync();
        }

        ViewBag.TypeOptions = GetTypeOptions(category);

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoomIssueViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TypeOptions = GetTypeOptions(vm.Category);

            if (!vm.RoomId.HasValue)
            {
                ViewBag.Rooms = await _context.Rooms
                    .OrderBy(r => r.Number)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"Room {r.Number}"
                    })
                    .ToListAsync();
            }

            return View(vm);
        }

        var user = await _userManager.GetUserAsync(User);

        var issue = new RoomIssue
        {
            RoomId = vm.RoomId,
            Category = vm.Category,
            TypeKey = vm.TypeKey,
            Description = vm.Description,
            Status = IssueStatus.New,
            ReportedByUserId = user?.Id,
            ReportedByUserName = User.Identity?.Name
        };

        _context.RoomIssues.Add(issue);

        // Optional: make Maintenance also set room status immediately
        if (vm.Category == IssueCategory.Maintenance && vm.RoomId.HasValue)
        {
            var room = await _context.Rooms.FindAsync(vm.RoomId.Value);
            if (room != null)
                room.Status = RoomStatus.Maintenance;
        }

        // Optional: housekeeping issues mark NeedsDailyCleaning
        if (vm.Category == IssueCategory.Housekeeping && vm.RoomId.HasValue)
        {
            var room = await _context.Rooms.FindAsync(vm.RoomId.Value);
            if (room != null)
                room.NeedsDailyCleaning = true;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Resolve(int id)
    {
        var issue = await _context.RoomIssues
            .Include(i => i.Room)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null) return NotFound();

        issue.Status = IssueStatus.Resolved;
        issue.ResolvedAt = DateTime.Now;

        // Optional: if it was Maintenance, restore room to Available/Cleaning logic (your choice)
        // Example: only clear maintenance if no other open maintenance issues remain
        if (issue.Category == IssueCategory.Maintenance && issue.RoomId.HasValue)
        {
            bool hasOtherOpen = await _context.RoomIssues.AnyAsync(i =>
                i.RoomId == issue.RoomId &&
                i.Category == IssueCategory.Maintenance &&
                i.Status != IssueStatus.Resolved &&
                i.Id != issue.Id);

            if (!hasOtherOpen && issue.Room != null)
                issue.Room.Status = RoomStatus.Available; // or Cleaning, depending on your workflow
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); // if you add Index page for issues
    }

    private static List<SelectListItem> GetTypeOptions(IssueCategory category)
    {
        // You can move this to a service later
        return category switch
        {
            IssueCategory.Maintenance => new()
            {
                new("Burst pipe", "BurstPipe"),
                new("AC not working", "ACNotWorking"),
                new("No hot water", "NoHotWater"),
                new("Electrical issue", "ElectricalIssue"),
                new("Other", "Other")
            },
            IssueCategory.Housekeeping => new()
            {
                new("Spill / stain", "Spill"),
                new("Extra towels", "ExtraTowels"),
                new("Extra bedding", "ExtraBedding"),
                new("Room needs cleaning", "DailyClean"),
                new("Other", "Other")
            },
            IssueCategory.Security => new()
            {
                new("Weird noise", "WeirdNoise"),
                new("Scream / shouting", "Scream"),
                new("Breaking things", "BreakingThings"),
                new("Suspicious person", "SuspiciousPerson"),
                new("Other", "Other")
            },
            _ => new() { new("Other", "Other") }
        };
    }
}
