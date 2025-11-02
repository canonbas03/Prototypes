using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

public class GuideController : Controller
{
    private readonly RouteService _routeService;

    public GuideController(RouteService routeService)
    {
        _routeService = routeService;
    }

    // Example URL: /Guide?offsetSeconds=85&side=left
    public IActionResult Index(int? offsetSeconds, string? side, string? direction, bool gps = false, int? gpsOffsetSeconds = null)
    {
        // Default: seat side both
        var seatSide = (side ?? "both").ToLowerInvariant();
        var dir = (direction ?? "forward").ToLowerInvariant();

        // Determine current offset (simulation-friendly):
        // Priority: if gps==true and gpsOffsetSeconds provided -> use that
        // else use offsetSeconds if provided -> simulation
        // else use DateTime.Now - journeyStart simulation (we'll just map current second of UTC minute to offset for demo)

        TimeSpan currentOffset;
        if (gps && gpsOffsetSeconds.HasValue)
        {
            currentOffset = TimeSpan.FromSeconds(gpsOffsetSeconds.Value);
        }
        else if (offsetSeconds.HasValue)
        {
            currentOffset = TimeSpan.FromSeconds(offsetSeconds.Value);
        }
        else
        {
            // Demo default: use seconds elapsed since application start (or second of minute) as simple moving time
            // To keep deterministic in demo, we'll use seconds since midnight UTC modulo 300s
            var seconds = DateTime.UtcNow.TimeOfDay.TotalSeconds % 300;
            currentOffset = TimeSpan.FromSeconds(seconds);
        }

        var active = _routeService.GetActivePoint(currentOffset, seatSide, dir);

        var model = new GuideViewModel
        {
            AllPoints = _routeService.AllPoints().ToList(),
            CurrentOffset = currentOffset,
            SeatSide = seatSide,
            Direction = dir,
            ActivePoint = active,
            UsingGps = gps
        };

        return View(model);
    }
}