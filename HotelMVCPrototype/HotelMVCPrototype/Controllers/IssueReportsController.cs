using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class IssueReportsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public IssueReportsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? roomId)
    {
        var vm = new CreateIssueReportViewModel
        {
            RoomId = roomId,
            Category = IssueCategory.Security // default
        };

        if (roomId.HasValue)
        {
            var room = await _context.Rooms
                .Where(r => r.Id == roomId.Value)
                .Select(r => new { r.Number })
                .FirstOrDefaultAsync();

            if (room != null) vm.RoomNumber = room.Number;
        }
        else
        {
            vm.Rooms = await _context.Rooms
                .OrderBy(r => r.Floor).ThenBy(r => r.Number)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"Room {r.Number} (Floor {r.Floor})"
                })
                .ToListAsync();
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateIssueReportViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            // reload rooms if needed
            if (!vm.RoomId.HasValue)
            {
                vm.Rooms = await _context.Rooms
                    .OrderBy(r => r.Floor).ThenBy(r => r.Number)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"Room {r.Number} (Floor {r.Floor})"
                    })
                    .ToListAsync();
            }
            return View(vm);
        }

        var user = await _userManager.GetUserAsync(User);

        // ✅ Map the unified UI to the right DB table
        // Keep it simple for now: create SecurityIncident for "Security",
        // create MaintenanceTicket for "Maintenance",
        // create HousekeepingTicket for "Housekeeping".
        // If you don't have those entities yet, you can start with SecurityIncident only.

        if (vm.Category == IssueCategory.Security)
        {
            var incident = new SecurityIncident
            {
                RoomId = vm.RoomId,
                Description = vm.Description,
                ReportedByUserId = user!.Id,
                Type = Enum.Parse<SecurityIncidentType>(vm.TypeKey) // keys match enum names
            };

            _context.SecurityIncidents.Add(incident);
            await _context.SaveChangesAsync();
        }
        else if (vm.Category == IssueCategory.Maintenance)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == vm.RoomId);
            room.Status = RoomStatus.Maintenance;

            await _context.SaveChangesAsync();
            // TODO: create Maintenance entity
            // var ticket = new MaintenanceTicket { ... Type = Enum.Parse<MaintenanceType>(vm.TypeKey) ... };
            // _context.MaintenanceTickets.Add(ticket);
            // await _context.SaveChangesAsync();
        }
        else if (vm.Category == IssueCategory.Housekeeping)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == vm.RoomId);
            room.NeedsDailyCleaning = true;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Home");
    }
}
