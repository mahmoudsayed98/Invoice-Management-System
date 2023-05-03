using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Authorization;

namespace IdentityApp.Data
{
    public class SeedData
    {
        public static async Task initialize(
            IServiceProvider serviceProvider, 
            string password = "Task@1234")
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()));
            {
                //manager
                var managerUid = await EnsureUser(serviceProvider, "manager@demo.com", password);
                await EnsureRole(serviceProvider, managerUid, Constants.InvoiceManagersRole);

                //admin
                var adminUid = await EnsureUser(serviceProvider, "admin@demo.com", password);
                await EnsureRole(serviceProvider, adminUid, Constants.InvoiceAdminRole);
            }
        }

        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider, 
            string username, string initPw)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(username);

            if(user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true

                };
            var result = await userManager.CreateAsync(user, initPw);
            }
            if (user == null)
                throw new Exception("User didnt Created ");

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;

            if(await roleManager.RoleExistsAsync(role) == false )
            {
               ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
                throw new Exception("User not found");


            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
