using Microsoft.EntityFrameworkCore;

namespace Lar.TesteDotNet.Shared.Interfaces.Database;

public interface IDbContext : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}