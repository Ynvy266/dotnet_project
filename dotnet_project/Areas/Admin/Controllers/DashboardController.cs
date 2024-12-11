using dotnet_project.Repository;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Dashboard")]
    public class DashboardController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DashboardController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var countProduct = _dataContext.Products.Count();
            var countOrder = _dataContext.Orders.Count();
            var countCategory = _dataContext.Categories.Count();
            var countBrand = _dataContext.Brands.Count();
            var countUser = _dataContext.Users.Count();

            ViewBag.ProductCount = countProduct;
            ViewBag.OrderCount = countOrder;
            ViewBag.CategoryCount = countCategory;
            ViewBag.BrandCount = countBrand;
            ViewBag.UserCount = countUser;

            return View();
        }
    }
}
