using Microsoft.AspNetCore.Identity;

namespace Mango.Services.OpenId.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
