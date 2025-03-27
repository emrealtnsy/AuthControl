using System.Linq.Expressions;

namespace AuthControl.Infrastructure.Persistence.Repository;

public interface IRepository<T> where T : class
{
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);
    IQueryable<T> Queryable();
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}