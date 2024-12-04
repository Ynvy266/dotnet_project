using dotnet_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    public class UserController : Controller
    {
        private readonly UserManager<AspUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<AspUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.OrderByDescending(p => p.Id).ToListAsync());
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AspUserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AspUserModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddIdentityErrors(result);
                    return View(user);
                }
            }
            else
            {
                TempData["error"] = "Model has some errors.";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return View("Error");
            }
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id, AspUserModel user)
        {
            var existUser = await _userManager.FindByIdAsync(id);
            if (existUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Update other properties excluding password
                existUser.UserName = user.UserName;
                existUser.Email = user.Email;
                existUser.PhoneNumber = user.PhoneNumber;
                existUser.RoleId = user.RoleId;

                var updateUser = await _userManager.UpdateAsync(existUser);
                if (updateUser.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddIdentityErrors(updateUser);
                    return View(existUser);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            //Validation Model failed
            TempData["Error"] = "Model has some errors.";
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            string errorMessage = string.Join("\n", errors);
            return View(existUser);
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
