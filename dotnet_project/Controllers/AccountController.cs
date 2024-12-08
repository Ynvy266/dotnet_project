using dotnet_project.Areas.Admin.Repository;
using dotnet_project.Models;
using dotnet_project.Models.ViewModels;
using dotnet_project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AspUserModel> _userManager;
        private SignInManager<AspUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;

        public AccountController(SignInManager<AspUserModel> signInManager,
            UserManager<AspUserModel> userManager,
            IEmailSender emailSender,
            DataContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _dataContext = context;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    TempData["success"] = "Login Successfully!";
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Incorrect username or password.");
            }
            return View(loginVM);
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
                AspUserModel newUser = new AspUserModel { UserName = user.Username, Email = user.Email };
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

        public async Task<IActionResult> ForgotPassword(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMailForgotPass(AspUserModel user)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            
            if (checkMail == null)
            {
                TempData["Error"] = "Email not found.";
                return RedirectToAction("ForgotPassword", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                //Update token to user
                checkMail.Token = token;
                _dataContext.Update(checkMail);
                await _dataContext.SaveChangesAsync();
                var receiver = checkMail.Email;
                var subject = "Reset your password";
                var message = "Click the following link to reset your password: " +
                    $"{Request.Scheme}://{Request.Host}/Account/NewPassword" +
                    $"?email=" + checkMail.Email + "&token=" + token;
                // $"<a href='{Request.Scheme}://{Request.Host}/Account/NewPass?email={checkMail.Email}&token={token}'>Reset Password</a>"
                await _emailSender.SendEmailAsync(receiver, subject, message);
            }
            TempData["success"] = "Check your email for a link to reset your password. If it doesn't appear within a few minutes, check your spam folder";
            return RedirectToAction("ForgotPassword", "Account");
        }

        public async Task<IActionResult> NewPassword(AspUserModel user, string token)
        {
            var checkUser = await _userManager.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                ViewBag.Email = checkUser.Email;
                ViewBag.Token = token;
            }
            else
            {
                TempData["error"] = "Email not found or invalid token.";
                return RedirectToAction("ForgotPassword", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNewPassword(AspUserModel user, string token)
        {
            var checkUser = await _userManager.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                //Update user with new password and token
                string newToken = Guid.NewGuid().ToString();
                //Hash the new password
                var passwordHasher = new PasswordHasher<AspUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkUser, user.PasswordHash);

                checkUser.PasswordHash = passwordHash;
                checkUser.Token = newToken;

                await _userManager.UpdateAsync(checkUser);
                TempData["success"] = "Successfully updated the new password!";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "Email not found or invalid token.";
                return RedirectToAction("ForgotPassword", "Account");
            }

            return View();
        }
    }
}