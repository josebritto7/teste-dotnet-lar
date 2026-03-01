using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Interfaces;

namespace Lar.TesteDotNet.Shared.Interfaces.Database;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity, IAggregateRoot;

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}