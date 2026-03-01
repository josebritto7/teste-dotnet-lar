using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lar.TesteDotNet.Infrastructure.Database;
using Lar.TesteDotNet.Presentation;

namespace Lar.TesteDotNet.Tests.Integration.Common;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _connection ??= new SqliteConnection("DataSource=:memory:");
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            services.AddLogging(lb =>
            {
                lb.ClearProviders();
                lb.AddConsole();
                lb.SetMinimumLevel(LogLevel.Information);
            });

            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                d.ServiceType == typeof(AppDbContext)).ToList();

            foreach (var d in descriptors)
                services.Remove(d);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection, b => b.MigrationsAssembly("Lar.TesteDotNet.Infrastructure"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection?.Dispose();
            _connection = null;
        }

        base.Dispose(disposing);
    }
}
