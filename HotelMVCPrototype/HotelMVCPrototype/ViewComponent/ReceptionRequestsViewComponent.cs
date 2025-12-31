using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelMVCPrototype.Data;
using HotelMVCPrototype.Models.Enums;

public class ReceptionRequestsViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ReceptionRequestsViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var requests = await _context.ServiceRequests
            .Include(r => r.Room)
            .Include(r => r.Items)
                .ThenInclude(i => i.RequestItem)
            .Where(r => r.Status == ServiceRequestStatus.New)
            .OrderBy(r => r.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View(requests);
    }
}
