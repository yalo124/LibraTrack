using LibraTrack.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraTrack.Data;
public class RoleSeeder
{
    private static async Task CreateUserAsync(UserManager<ApplicationUser> userManager, string email, string fullname, string password, string role)
    {
        if (await userManager.FindByEmailAsync(email) != null) return;

        var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true, FullName = fullname };
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach(var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        await CreateUserAsync(userManager, "neil_admin@library.text", "System Admin", "NeilAdmin123", AppRoles.Admin);
        await CreateUserAsync(userManager, "adrian_librarian@library.text", "Librarian", "AdrianLibrarian123", AppRoles.Librarian);
    }
}
