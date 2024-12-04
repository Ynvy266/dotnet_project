using Microsoft.AspNetCore.Identity;

namespace dotnet_project.Models
{
    public class AspUserModel : IdentityUser
    {
        public string RoleId { get; set; }

    }
}
