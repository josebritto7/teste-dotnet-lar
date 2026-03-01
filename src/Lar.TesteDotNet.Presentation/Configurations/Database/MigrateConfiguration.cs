using Microsoft.Data.Sqlite;
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
            var resetOnStartup = app.Configuration.GetValue<bool>("Database:ResetOnStartup");
            if (resetOnStartup)
                db.Database.EnsureDeleted();

            EnsureSqliteCreated(db);
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

    private static void EnsureSqliteCreated(AppDbContext db)
    {
        try
        {
            db.Database.EnsureCreated();
        }
        catch (SqliteException ex) when (IsTableAlreadyExists(ex))
        {
        }
    }

    private static bool IsTableAlreadyExists(SqliteException exception)
    {
        return exception.SqliteErrorCode == 1 &&
               exception.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase);
    }
}
