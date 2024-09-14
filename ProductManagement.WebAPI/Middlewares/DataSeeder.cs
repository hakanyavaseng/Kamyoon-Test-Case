using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.Consts;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Entities.Common;
using ProductManagement.Persistence.Contexts;

namespace ProductManagement.WebAPI.Middlewares;

public class DataSeeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //Check if the database is created
            await dbContext.Database.EnsureCreatedAsync();

            //Apply any pending migrations
            await dbContext.Database.MigrateAsync();
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var _passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

            if (_roleManager.Roles.Any() || _userManager.Users.Any())
                return;

            // Seed roles
            var adminRole = new AppRole
            {
                Id = Guid.Parse("19c02d4d-4ae2-4d09-8306-ae6946b5b20b"),
                Name = RoleConsts.Admin
            };

            var userRole = new AppRole
            {
                Id = Guid.Parse("c833bd7a-a95a-4e2e-8afb-12caeaba1e99"),
                Name = RoleConsts.User
            };

            if (!await _roleManager.RoleExistsAsync(RoleConsts.Admin))
                await _roleManager.CreateAsync(adminRole);

            if (!await _roleManager.RoleExistsAsync(RoleConsts.User))
                await _roleManager.CreateAsync(userRole);

            // Seed users
            var user = new AppUser
            {
                Id = Guid.Parse("6b1accff-7c83-4bbb-9fb5-16efa8461160"),
                UserName = "user",
                Email = "user@gmail.com"
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "P@ssw0rd");

            var admin = new AppUser
            {
                Id = Guid.Parse("5deb92b3-c39d-4f26-9e7c-897e3abebd37"),
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            // Add users and assign roles
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, RoleConsts.User);
            }

            if (await _userManager.FindByEmailAsync(admin.Email) == null)
            {
                await _userManager.CreateAsync(admin);
                await _userManager.AddToRoleAsync(admin, RoleConsts.Admin);
            }
        }
    }
}