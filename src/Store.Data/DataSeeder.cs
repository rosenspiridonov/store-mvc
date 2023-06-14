using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Store.Data.Entities;
using Store.Data;

using static Store.Commons.Constants;

namespace Store.Data
{
    public class DataSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public DataSeeder(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedOrderStatusesAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new List<string>() { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var identityRole = new IdentityRole(role);
                    await _roleManager.CreateAsync(identityRole);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _userManager.FindByNameAsync("admin@example.com") == null)
            {
                var user = new IdentityUser()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                };

                string adminPassword = _configuration["AdminCredentials:Password"];
                var result = await _userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private async Task SeedOrderStatusesAsync()
        {
            var statuses = new List<string> { "Pending", "Canceled", "Completed" };

            foreach (var status in statuses)
            {
                if (!await _context.OrderStatuses.AnyAsync(os => os.Name == status))
                {
                    _context.OrderStatuses.Add(new OrderStatus { Name = status });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
