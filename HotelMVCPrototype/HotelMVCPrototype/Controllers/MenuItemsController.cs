using HotelMVCPrototype.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MenuItemsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MenuItemsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }


    public async Task<IActionResult> Index()
    {
        var items = await _context.MenuItems
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();

        return View(items);
    }
    public IActionResult Create()
    {
        return View(new MenuItemCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItemCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        string? imagePath = null;

        if (model.Image != null)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images/menu");
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.Image.CopyToAsync(stream);

            imagePath = "/images/menu/" + fileName;
        }

        var item = new MenuItem
        {
            Name = model.Name,
            Price = model.Price,
            Category = model.Category,
            IsVegan = model.IsVegan,
            ImagePath = imagePath
        };

        _context.MenuItems.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
