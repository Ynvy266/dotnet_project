using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class ProductQuantityModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="The quantity field is required.")]
        public int Quantity { get; set; }

        public long ProductId { get; set; }
        public DateTime DateCreated { get; set; }

        public int Status { get; set; }
    }
}
