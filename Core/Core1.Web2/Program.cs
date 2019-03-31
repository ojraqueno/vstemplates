using Core1.Infrastructure.Data;
using Core1.Infrastructure.Identity;
using Core1.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace Core1.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WrapInLog(() =>
            {
                var host = CreateWebHostBuilder(args).Build();

                SeedDatabase(host);

                host.Run();
            });
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();

        private static void SeedDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<AppIdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppIdentityRole>>();
                DbInitializer.Seed(db, userManager, roleManager);
            }
        }

        private static void WrapInLog(Action action)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                action();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}