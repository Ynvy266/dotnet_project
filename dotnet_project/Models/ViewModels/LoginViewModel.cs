using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The email field is required."), EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "The password field is required.")]
        //password encryption
        public string Password { get; set; }

        public string ReturnURL { get; set; }
    }
}
