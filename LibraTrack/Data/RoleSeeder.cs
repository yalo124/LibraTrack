using LibraTrack.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraTrack.Data;
public class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var role_Manager = services.GetRequiredService<RoleManager<IdentityRole>>();

        foreach(var role in App_Roles.All)
        {
            if (!await role_Manager.RoleExistsAsync(role))
            {
                await role_Manager.CreateAsync(new IdentityRole(role));
            }
        }
    }

}
