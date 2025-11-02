using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class GuideController : Controller
{
    private readonly RouteService _routeService;

    public GuideController(RouteService routeService)
    {
        _routeService = routeService;
    }

    // Example URL: /Guide?gps=true
    public IActionResult Index(bool gps = false)
    {
        // We only proceed if GPS is enabled
        if (!gps)
        {
            return Content("This prototype works only with GPS enabled. Append '?gps=true' to the URL.");
        }

        var model = new GuideViewModel
        {
            AllPoints = _routeService.AllPoints().ToList(),
            UsingGps = true
        };

        return View(model);
    }
}
