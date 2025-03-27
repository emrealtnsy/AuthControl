using AuthControl.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthControl.Infrastructure.Persistence.Database;

public sealed class DatabaseSetup(AuthControlDbContext context)
{
    public async Task SetupAsync()
    {
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
            await context.Database.MigrateAsync();
    }
}