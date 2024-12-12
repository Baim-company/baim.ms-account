using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Models.Entities.Staffs;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data.Initialization;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AgileDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.Database.MigrateAsync();


        await InitializeRolesAsync(dbContext);
    }

    private static async Task InitializeRolesAsync(AgileDbContext dbContext)
    {
        var roles = new[] { "Admin", "Staff", "UserAdmin", "User" };
        foreach (var roleName in roles)
        {
            if (!dbContext.Roles.Any(r => r.RoleName == roleName))
            {
                dbContext.Roles.Add(new Role { RoleName = roleName });
            }
        }

        await dbContext.SaveChangesAsync();
    }
}