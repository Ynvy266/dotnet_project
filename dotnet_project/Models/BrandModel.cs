using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public int Status { get; set; }
    }
}
