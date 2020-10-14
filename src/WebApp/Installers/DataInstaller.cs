using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Data;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Installers {
    public class DataInstaller : IInstaller {
        public void InstallServices(IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsHistoryTable("MigrationsHistory", "EF")));
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DataContext>();
            services.AddScoped<IPostService, PostService>();
        }
    }
}
