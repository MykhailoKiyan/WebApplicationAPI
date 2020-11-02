using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI {
    public class Program {
        public static async Task Main(string[] args) {

            using var host = CreateWebHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                // await context.Database.MigrateAsync();
                if (!await roleManager.RoleExistsAsync("Admin")) {
                    var adminRole = new Role { Name = "Admin" };
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync("Poster")) {
                    var posterRole = new Role { Name = "Poster" };
                    await roleManager.CreateAsync(posterRole);
                }

                await host.RunAsync();
            } catch (Exception ex) {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
                throw;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
