/*
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain.Identity;
using WebApplicationAPI.Services;

namespace WebApplicationAPI {
    public partial class Startup {
        partial void DataConfigureServices(IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsHistoryTable("MigrationsHistory", "EF")));
            //services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DataContext>();

            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddScoped<IPostService, PostService>();
        }
    }
}
*/
