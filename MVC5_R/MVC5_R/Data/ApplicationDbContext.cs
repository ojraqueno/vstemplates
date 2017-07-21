using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_R.Models;
using System.Data.Entity;

namespace MVC5_R.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("ApplicationDbContext", false)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}