using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The username field is required.")]
        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "The password field is required.")]
        //password encryption
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
