using Microsoft.AspNetCore.Identity;

namespace CMSApp.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public string JobTitle { get; set; }

    }
}
