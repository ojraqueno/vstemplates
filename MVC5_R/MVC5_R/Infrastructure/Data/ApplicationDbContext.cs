﻿using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_R.Models;
using System.Data.Entity;

namespace MVC5_R.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() : base("ApplicationDbContext", false)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<CustomRole> CustomRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}