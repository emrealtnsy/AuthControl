using AuthControl.Infrastructure.Persistence.Database.Seed;

namespace AuthControl.Infrastructure.Persistence.Database;

public sealed class DatabaseSeeder(DatabaseSetup databaseSetup, SeedData seedData)
{
    public async Task SeedAsync()
    {
        await databaseSetup.SetupAsync();
        await seedData.InitializeAsync();
    }
}