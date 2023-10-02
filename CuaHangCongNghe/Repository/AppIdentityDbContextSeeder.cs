using CuaHangCongNghe.Repository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CuaHangCongNghe
{
    public class AppIdentityDbContextSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var defaultUser = new ApplicationUser { Name = "vanSon", UserName = "vanSon" ,Address = "kien truc ha noi" };
            if ((await userManager.FindByNameAsync("vanSon")) == null)
            {
                await userManager.CreateAsync(defaultUser, "Pass@word1");
                var roleName = "Admin";
                await roleManager.CreateAsync(new IdentityRole(roleName));
                await userManager.AddToRoleAsync(defaultUser, roleName);
            }
        }
    }
}
