using dotnet_project.Models;
using dotnet_project.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AspUserModel> _userManager;
        private SignInManager<AspUserModel> _signInManager;
        public AccountController(SignInManager<AspUserModel> signInManager, UserManager<AspUserModel> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnURL = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(login.ReturnURL ?? "/");
                }
                ModelState.AddModelError("", "Email or password does not match.");
            }
            return View(login);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AspUserModel newUser = new AspUserModel { UserName = user.Username, Email = user.Email};
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Account registration successful!";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
    }
}
