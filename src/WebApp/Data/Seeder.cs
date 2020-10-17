using Microsoft.AspNetCore.Identity;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Data {
    public static class Seeder {

        public static void Run(DataContext dbContext, UserManager<User> userManager, RoleManager<User> roleManager) { }
    }
}
