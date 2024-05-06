using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.Manage.ViewModels;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "admin",
            };

            var r = await _userManager.CreateAsync(admin, "Admin123");

            return Json(r);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVM)
        {
            AppUser admin = await _userManager.FindByNameAsync(loginVM.UserName);

            if (admin == null)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(admin, loginVM.Password,false,false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }


            return RedirectToAction("index", "dashboard");
        }

        public IActionResult GetName()
        {
            var username = User.Identity.Name;

            return Json(User.Identity);
        }
    }
}
