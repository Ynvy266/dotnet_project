using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the category name.")]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }

        public int Status { get; set; }
    }
}
