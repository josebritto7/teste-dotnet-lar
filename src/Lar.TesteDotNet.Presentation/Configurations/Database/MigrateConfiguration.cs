using Microsoft.EntityFrameworkCore;
using Lar.TesteDotNet.Infrastructure.Database;

namespace Lar.TesteDotNet.Presentation.Configurations.Database;

public static class MigrateConfiguration
{
    public static void Migrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Database.IsSqlite())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return;
        }

        var allMigrations = db.Database.GetMigrations().ToList();
        if (allMigrations.Any())
        {
            var pending = db.Database.GetPendingMigrations();
            if (pending.Any()) db.Database.Migrate();
            return;
        }

        db.Database.EnsureCreated();
    }
}
