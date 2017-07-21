using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_R.Models;

namespace MVC5_R.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}