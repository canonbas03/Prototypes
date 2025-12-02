using System.Diagnostics;
using HotelMVCPrototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace HotelMVCPrototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // Get the first role of the logged-in user
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                if (!string.IsNullOrEmpty(roleName))
                {
                    // Redirect to role controller index
                    return RedirectToAction("Index", roleName);
                }
            }

            // Default for non-logged-in users
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
