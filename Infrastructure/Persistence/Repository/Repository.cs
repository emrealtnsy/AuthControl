using System.Linq.Expressions;
using AuthControl.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthControl.Infrastructure.Persistence.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AuthControlDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(AuthControlDbContext context)
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _dbContext.Set<T>();
    }
    
    public IQueryable<T> Queryable() => _dbSet.AsQueryable();

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, bool trackChanges, CancellationToken cancellationToken) =>
        trackChanges
            ? await _dbContext.Set<T>()
                .FirstOrDefaultAsync(predicate, cancellationToken)
            : await _dbContext.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken) =>
        trackChanges
            ? await _dbContext.Set<T>().ToListAsync(cancellationToken)
            : await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken) =>
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        await _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) =>
        await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
}