using dotnet_project.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if(!_context.Products.Any())
            {
                CategoryModel bowl = new CategoryModel { Name = "Bowl", Slug = "bowl", Description = "Abc", Status = 1 };
                CategoryModel plate = new CategoryModel { Name = "Plate", Slug = "plate", Description = "Abc", Status = 1 };

                BrandModel hanselmann = new BrandModel { Name = "Hanselmann", Slug = "hanselmann", Description = "Abc", Status = 1 };
                BrandModel nobrand = new BrandModel { Name = "No Brand", Slug = "nobrand", Description = "Abc", Status = 1 };

                _context.Products.AddRange(
                    new ProductModel { Name = "Breakfast Bowl", Slug = "breakfastbowl", Description = "Abc", Image = "1.jpg", Category = bowl, Brand = hanselmann, Price = 1058000 },
                    new ProductModel { Name = "No Brand Gray Plate", Slug = "grayplate", Description = "Abc", Image = "1.jpg", Category = plate, Brand = nobrand, Price = 150000 }
                );
                _context.SaveChanges();
            }
        }
    }
}
