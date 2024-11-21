using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
