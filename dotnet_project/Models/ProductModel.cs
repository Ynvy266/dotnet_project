using dotnet_project.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the product name.")]
        public string Name { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }
        public string Slug { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public string Image { get; set; } = "NoImage";

        [Required, Range(1, int.MaxValue, ErrorMessage ="Please choose a brand.")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a category.")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
