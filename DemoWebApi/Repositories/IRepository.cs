using DemoWebApi.Entities;
using System.Linq.Expressions;

namespace DemoWebApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(bool isNoTracking = false, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> predicate);
        IQueryable<T> GetDbSet();
    }
}
