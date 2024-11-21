using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the brand name.")]
        public string Name { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }

        public string Slug { get; set; }

        public int Status { get; set; }
    }
}
