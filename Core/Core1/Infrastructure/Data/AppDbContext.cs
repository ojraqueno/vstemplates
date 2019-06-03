using Core1.Infrastructure.Identity;
using Core1.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core1.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Log> Logs { get; set; }
    }
}