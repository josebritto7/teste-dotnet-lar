using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Interfaces;
using Lar.TesteDotNet.Shared.Interfaces;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(IDbContext dBContext)
    {
        _dbContext = dBContext;
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity, IAggregateRoot
    {
        var type = typeof(TEntity);

        if (_repositories.TryGetValue(type, out var repository)) return (IRepository<TEntity>)repository;

        _repositories[type] = new Repository<TEntity>(_dbContext);

        return (IRepository<TEntity>)_repositories[type];
    }
}