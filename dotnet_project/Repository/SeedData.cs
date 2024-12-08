using dotnet_project.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            //_context.Database.Migrate();
            if(!_context.Products.Any())
            {
                CategoryModel bowl = new CategoryModel { Name = "Bowl", Slug = "bowl", Status = 1 };
                CategoryModel plate = new CategoryModel { Name = "Plate", Slug = "plate", Status = 1 };

                BrandModel hanselmann = new BrandModel { Name = "Hanselmann", Slug = "hanselmann", Status = 1 };
                BrandModel nobrand = new BrandModel { Name = "No Brand", Slug = "nobrand", Status = 1 };

                _context.Products.AddRange(
                    new ProductModel { Name = "Breakfast Bowl", Slug = "breakfastbowl", Description = "Hand-thrown ceramic breakfast bowl. Quite possibly the most frequently used dish in your home.  This bowl is great for everything from rice or soup, to hot cereals and desserts.", 
                                       Image = "Hanselmann/1.jpg", Category = bowl, Brand = hanselmann, Price = 48 },
                    new ProductModel { Name = "No Brand Gray Plate", Slug = "grayplate", Description = "Traditional 10\" dinner size plate, and the largest of Hanselmann's three dinnerware plates. These durable stoneware plates are on the heavy side, designed for everyday use and washing.", 
                                       Image = "NoBrand/1.jpg", Category = plate, Brand = nobrand, Price = 6 }
                );
                _context.SaveChanges();
            }
        }
    }
}
