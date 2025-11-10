using System.Diagnostics;
using HotelMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HotelDbContext _context;

        public HomeController(ILogger<HomeController> logger, HotelDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RoomView()
        {
            var rooms = _context.Rooms.ToList();

            var totalPrice = rooms.Sum(r => r.Price);
            ViewBag.TotalPrice = totalPrice;

            return View(rooms);
        }

        public IActionResult CreateEditRoom(int? id)
        {
            if (id is not null)
            {
                var room = _context.Rooms.FirstOrDefault(r => r.ID == id);
                return View(room);
            }

            return View();
        }

        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.ID == id);

            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return RedirectToAction("RoomView");
        }

        public IActionResult CreateEditRoomForm(Room model)
        {
            if (model.ID == 0)
            {
                // We create
                _context.Rooms.Add(model);
            }
            else
            {
                _context.Rooms.Update(model);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
