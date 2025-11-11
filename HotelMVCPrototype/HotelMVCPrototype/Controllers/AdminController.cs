using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UserWithRolesViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var model = new List<UserWithRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserWithRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return View(model);
    }

    // GET: Show roles for a specific user
    public async Task<IActionResult> EditRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var allRoles = _roleManager.Roles.ToList();

        var model = new EditRolesViewModel
        {
            UserId = user.Id,
            Email = user.Email,
            Roles = allRoles.Select(r => new RoleCheckbox
            {
                RoleName = r.Name,
                IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
            }).ToList()
        };

        return View(model);
    }

    // POST: Save changes
    [HttpPost]
    public async Task<IActionResult> EditRoles(EditRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);

        // Add new roles
        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        // Remove unchecked roles
        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        return RedirectToAction("Index");
    }
}

