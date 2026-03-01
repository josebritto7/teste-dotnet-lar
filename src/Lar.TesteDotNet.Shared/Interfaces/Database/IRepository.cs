using System.Linq.Expressions;

namespace Lar.TesteDotNet.Shared.Interfaces.Database;

public interface IRepository<T>
{
    void Add<TEntity>(TEntity entity) where TEntity : T;

    void Delete<TEntity>(TEntity entity) where TEntity : T;

    void Update<TEntity>(TEntity entity) where TEntity : T;

    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetAll();

    bool Exists(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetWithCursorFiltering(DateTime? date, long? id, int limit,
        Expression<Func<T, bool>>? predicate = null);

    IQueryable<T> GetAllWithLimit(int limit, Expression<Func<T, bool>>? predicate = null);
}