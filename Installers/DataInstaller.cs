using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Data;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Installers {
  public class DataInstaller : IInstaller {
    public void InstallServices(
        IServiceCollection services
      , IConfiguration configuration
    ) {
      string connectionString = configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

      services
        .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<DataContext>();

      services.AddSingleton<IPostService, PostService>();
    }
  }
}