using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds;

public class DefaultRoles
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<string>
        {
            "Admin",
            "Mentor",
            "Student"
        };

        foreach (var role in roles)
        {
            var existingRole = await roleManager.FindByNameAsync(role);
            if (existingRole != null)
            {
                continue;
            }

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}