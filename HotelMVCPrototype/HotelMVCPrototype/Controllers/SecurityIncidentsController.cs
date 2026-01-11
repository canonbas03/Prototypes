using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class SecurityIncidentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SecurityIncidentsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /SecurityIncidents/Create
    public IActionResult Create(int? roomId)
    {
        return View(new CreateSecurityIncidentViewModel
        {
            RoomId = roomId
        });
    }

    // POST: /SecurityIncidents/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSecurityIncidentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        var incident = new SecurityIncident
        {
            Type = model.Type,
            Description = model.Description,
            RoomId = model.RoomId,
            ReportedByUserId = user.Id
        };

        _context.SecurityIncidents.Add(incident);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Reception"); // or dashboard
    }
}
