using MVC5_R.Models;
using System.Data.Entity.ModelConfiguration;

namespace MVC5_R.Infrastructure.Data.EntityConfiguration
{
    public class CustomRoleConfiguration : EntityTypeConfiguration<CustomRole>
    {
        public CustomRoleConfiguration()
        {
            HasMany(u => u.Users)
                .WithMany(r => r.CustomRoles)
                .Map(config =>
                {
                    config.ToTable("UserCustomRoles");
                    config.MapLeftKey("CustomRoleId");
                    config.MapRightKey("UserId");
                });
        }
    }
}