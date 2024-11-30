using dotnet_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AspUserModel> _userManager;
        private SignInManager<AspUserModel> _signInManger;
        public AccountController(SignInManager<AspUserModel> signInManager, UserManager<AspUserModel> userManager) { 
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
