using AuthControl.Domain.Constants;
using AuthControl.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthControl.Infrastructure.Persistence.Database.Seed;

public sealed class SeedData(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
{
    public async Task InitializeAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (var roleName in RoleConstants.Roles)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;
            var role = new IdentityRole(roleName);
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
                await EnsureSuccessAsync(result, "“Failed to add the role to the database.");
        } 
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminUserName = "adminauthcontrol";
        const string adminEmail = "admin@authcontrol.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByNameAsync(adminUserName);
        
        if (adminUser == null)
        {
            adminUser = new User
            {
                Name = "Admin",
                Surname = "Authcontrol",
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
            await EnsureSuccessAsync(createUserResult, "Failed to create admin user.");
            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            await EnsureSuccessAsync(addToRoleResult, "Failed to assign the admin role to the user.");
        }
    }

    private static Task EnsureSuccessAsync(IdentityResult result, string errorMessage)
    {
        if (result.Succeeded)
            return Task.CompletedTask;

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"{errorMessage} Errors: {errors}");
    }
}