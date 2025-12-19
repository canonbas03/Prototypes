using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models;
using HotelMVCPrototype.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GuestRequestsController : Controller
{
    private readonly ApplicationDbContext _context;

    public GuestRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // STEP 1: Show request form
    public IActionResult Index(int roomId)
    {
        ViewBag.RoomId = roomId;

        ViewBag.Items = new List<string>
        {
            "Toothbrush",
            "Shaving Cream",
            "Iron",
            "Extra Towels",
            "Extra Pillow",
            "Remote Control Batteries"
        };

        return View();
    }

    // STEP 2: Submit request
    [HttpPost]
    public async Task<IActionResult> PlaceRequest(
        int roomId,
        Dictionary<string, int> items)
    {
        if (items == null || !items.Any(i => i.Value > 0))
            return BadRequest("No items selected.");

        var request = new ServiceRequest
        {
            RoomId = roomId,
            Status = ServiceRequestStatus.New,
            Items = new List<ServiceRequestItem>()
        };

        foreach (var item in items.Where(i => i.Value > 0))
        {
            request.Items.Add(new ServiceRequestItem
            {
                ItemName = item.Key,
                Quantity = item.Value
            });
        }

        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync();

        // ✅ Redirect to its OWN ThankYou page
        return RedirectToAction(nameof(ThankYou), new { id = request.Id });
    }

    [HttpGet]
    public async Task<IActionResult> ThankYou(int id)
    {
        var request = await _context.ServiceRequests
            .Include(r => r.Items)
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null)
            return NotFound();

        return View(request);
    }
}
