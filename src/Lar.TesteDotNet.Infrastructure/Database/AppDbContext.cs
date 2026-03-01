using Microsoft.EntityFrameworkCore;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Infrastructure.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IDbContext
{
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Telefone> Telefones => Set<Telefone>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
