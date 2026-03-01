using Microsoft.EntityFrameworkCore;
using Lar.TesteDotNet.Infrastructure.Database;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Presentation.Configurations.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection") ??
            "Data Source=lar.testedotnet.db";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
