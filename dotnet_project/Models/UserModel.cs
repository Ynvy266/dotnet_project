using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The email field is required."), EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "The password field is required.")]
        //password encryption
        public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match.")]
        //public string ConfirmPassword { get; set; }
    }
}
