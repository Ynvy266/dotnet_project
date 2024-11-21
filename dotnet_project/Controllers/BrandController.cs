using dotnet_project.Models;
using dotnet_project.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;

        }

        public async Task<IActionResult> Index(string Slug = "")
        {
            BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
            if (brand == null) return RedirectToAction("Index");
            var productsByBrand = _dataContext.Products.Where(b => b.BrandId == brand.Id);
            return View(await productsByBrand.OrderByDescending(b => b.Id).ToListAsync());
        }
    }
}
