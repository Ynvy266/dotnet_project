using dotnet_project.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext context)
        {
            _dataContext = context;
        }

        public IActionResult Index()
        {
            return View();  
        }

        public async Task<IActionResult> Search(string searchItem)
        {
            var products = await _dataContext.Products
               .Where(p => p.Name.Contains(searchItem) || p.Description.Contains(searchItem))
               .ToListAsync();
            ViewBag.Keyword = searchItem;
            return View(products);
        }

        public async Task<IActionResult> Details(int Id)
        {
            if(Id == null) return RedirectToAction("Index");
            var productsById = _dataContext.Products.Where(i => i.Id == Id).FirstOrDefault();
            return View(productsById);
        }
    }
}
