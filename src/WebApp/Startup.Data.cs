using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI {
    public partial class Startup {
        partial void DataConfigureServices(IServiceCollection services) {
            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection"),
                    options => options.MigrationsHistoryTable("MigrationsHistory", "EF"));
            });

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
        }
    }
}
