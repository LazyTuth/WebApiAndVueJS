using Microsoft.AspNetCore.Identity;

namespace DemoApp.API.Data
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}