using dotnet_project.Models;
using dotnet_project.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace dotnet_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index(string Slug = "", string sort_by = "",string startprice = "",string endprice = "")
        {
			ViewBag.Slug = Slug;
			var products = _dataContext.Products.ToList();

			var count = products.Count();
			if (count > 0)
			{
				if (sort_by == "price_increase")
				{
					products = products.OrderBy(p => p.Price).ToList();
				}
				else if (sort_by == "price_decrease")
				{
					products = products.OrderByDescending(p => p.Price).ToList();
				}
				else if (sort_by == "price_newest")
				{
					products = products.OrderByDescending(p => p.Id).ToList();
				}
				else if (sort_by == "price_oldest")
				{
					products = products.OrderBy(p => p.Id).ToList();
				}
				else if (startprice != "" && endprice != "")
				{
					decimal startPriveValue;
					decimal endPriceValue;
					if (decimal.TryParse(startprice, out startPriveValue) && decimal.TryParse(endprice, out endPriceValue))
					{
						products = products.Where(p => p.Price >= startPriveValue && p.Price <= endPriceValue).ToList();

					}
					else
					{
						products = products.OrderByDescending(p => p.Id).ToList();
					}
				}
				else
				{
					products = products.OrderByDescending(p => p.Id).ToList();
				}
			}
			//            productsByCategory = productsByCategory.OrderBy(p => p.Price);
			//var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);
			//return View(await productsByCategory.ToListAsync());

			return View(products);
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {

            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
