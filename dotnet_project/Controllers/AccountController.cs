using dotnet_project.Models;
using dotnet_project.Models.ViewModels;
using dotnet_project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_project.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AspUserModel> _userManager;
        private SignInManager<AspUserModel> _signInManager;
        private readonly DataContext _dataContext;
        public AccountController(SignInManager<AspUserModel> signInManager, UserManager<AspUserModel> userManager,
            DataContext context) 
        {
            _dataContext = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Email or password does not match.");
            }
            return View(loginVM);
        }

		public async Task<IActionResult> UpdateAccount()
		{
            if((bool)!User.Identity?.IsAuthenticated)
            {
                //user is not logged in, redirect to login
                return RedirectToAction("Login", "Account");  // replace ""Account" with your controller
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
			//get user by user email
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if(user == null)
            {
                return NotFound();
            }
			return View();
		}
        [HttpPost]
		public async Task<IActionResult> UpdateInfoAccount(AspUserModel user)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			//var userEmail = User.FindFirstValue(ClaimTypes.Email);
			//get user by user email
			var userById = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (userById == null)
			{
				return NotFound();
			}
            else
            {
                //Hash the new password
                var passwordHasher = new PasswordHasher<AspUserModel>();
                var passwordHash = passwordHasher.HashPassword(userById, user.PasswordHash);
                userById.PasswordHash = passwordHash;

                _dataContext.Update(userById);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update Account Information Successfully";
            }
            return RedirectToAction("UpdateAccount", "Account");
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

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
