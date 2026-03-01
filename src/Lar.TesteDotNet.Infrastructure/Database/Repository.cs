using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Interfaces;
using Lar.TesteDotNet.Shared.Interfaces;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Infrastructure.Database;

public class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    private readonly DbSet<T> _dbSet;

    public Repository(IDbContext dbContext)
    {
        _dbSet = dbContext.Set<T>();
    }

    public void Add<TEntity>(TEntity entity) where TEntity : T
    {
        _dbSet.Add(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : T
    {
        _dbSet.Remove(entity);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : T
    {
        _dbSet.Update(entity);
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking();
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.AsNoTracking().Any(predicate);
    }

    public IQueryable<T> GetWithCursorFiltering(
        DateTime? date,
        long? id,
        int limit,
        Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet
            .AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        return query.Where(e => e.CreatedAt < date || (e.CreatedAt == date && e.Id < id))
            .OrderByDescending(e => e.CreatedAt)
            .ThenByDescending(e => e.Id)
            .Take(limit + 1);
    }

    public IQueryable<T> GetAllWithLimit(int limit, Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet
            .AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        return query.OrderByDescending(e => e.CreatedAt)
            .ThenByDescending(e => e.Id)
            .Take(limit + 1);
    }
}