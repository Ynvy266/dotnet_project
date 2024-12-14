using dotnet_project.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models
{
    public class ProductModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The description field is required.")]
        public string Description { get; set; }
        public string Slug { get; set; }

        [Required(ErrorMessage = "The price field is required.")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
		[Required(ErrorMessage = "The capital price field is required.")]
        public decimal CapitalPrice { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }

		public string Image { get; set; } = "NoImage";

        [Required, Range(1, int.MaxValue, ErrorMessage ="Please choose a brand.")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a category.")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        [NotMapped]
        [FileExtension]
        [Required(ErrorMessage = "The image field is required.")]
        public IFormFile ImageUpload { get; set; }
    }
}
