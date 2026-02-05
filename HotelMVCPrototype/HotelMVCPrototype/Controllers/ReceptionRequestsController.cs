using HotelMVCPrototype.Data;
using HotelMVCPrototype.Hubs;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Reception, Admin")]
public class ReceptionRequestsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<HotelHub> _hub;

    public ReceptionRequestsController(ApplicationDbContext context, IHubContext<HotelHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    public async Task<IActionResult> Index()
    {
        var newRequests = await _context.ServiceRequests
            .Include(r => r.Room)
            .Include(r => r.Items)
                .ThenInclude(i => i.RequestItem)
            .Where(r => r.Status == ServiceRequestStatus.New)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();

        var completedRequests = await _context.ServiceRequests
            .Include(r => r.Room)
            .Include(r => r.Items)
                .ThenInclude(i => i.RequestItem)
            .Where(r => r.Status == ServiceRequestStatus.Completed)
            .OrderByDescending(r => r.CreatedAt)
            .Take(10)
            .ToListAsync();

        return View(new ReceptionRequestsIndexViewModel
        {
            NewRequests = newRequests,
            CompletedRequests = completedRequests
        });
    }

    public IActionResult PartialRequests()
    {
        return ViewComponent("ReceptionRequests");
    }


    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var request = await _context.ServiceRequests.FindAsync(id);
        if (request == null) return NotFound();

        request.Status = ServiceRequestStatus.Completed;
        request.CompletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        bool hasRequests = await _context.ServiceRequests.AnyAsync(r => r.RoomId == request.RoomId && r.Status == ServiceRequestStatus.New);

        if (!hasRequests)
        {

            await _hub.Clients.All.SendAsync("RequestCompleted", request.RoomId);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRequestItemViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        string? imagePath = null;

        if (model.Image != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/requests"
            );

            Directory.CreateDirectory(uploadPath);

            var fullPath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await model.Image.CopyToAsync(stream);

            imagePath = "/images/requests/" + fileName;
        }

        var item = new RequestItem
        {
            Name = model.Name,
            ImagePath = imagePath,
            IsActive = model.IsActive
        };

        _context.RequestItems.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
