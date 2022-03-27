using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using ReactBackend.Models;

namespace ReactBackend.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class RoleController : Controller
    {
        private readonly SignInManager<ReactBackendUser> _signInManager;
        private readonly UserManager<ReactBackendUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(SignInManager<ReactBackendUser> signInManager, UserManager<ReactBackendUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            ViewData["Users"] = new SelectList(_userManager.Users, "Id", "UserName");
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string user, string role)
        {
            var changeUser = await _userManager.FindByIdAsync(user);
            var roles = await _userManager.GetRolesAsync(changeUser);
            await _userManager.RemoveFromRolesAsync(changeUser, roles.ToArray());
            IdentityResult result = await _userManager.AddToRoleAsync(changeUser, role);
            if (result.Succeeded)
            {
                if (User.Identity.Name == changeUser.UserName)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(changeUser, isPersistent: false);
                }
                return RedirectToAction("Index", "Person");
            }
                
            ViewData["Message"] = "Couldn't change role for the user!";
            ViewData["Users"] = new SelectList(_userManager.Users, "Id", "UserName");
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

    }
}
