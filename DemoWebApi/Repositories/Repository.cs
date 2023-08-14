using DemoWebApi.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoWebApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext DbContext { get; }
        protected DbSet<T> DbSet { get; }

        public Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;

            DbSet = DbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var res = await DbContext.Set<T>().AddAsync(entity, cancellationToken);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);

            return res.Entity;
        }

        public async Task DeleteAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Set<T>().Remove(entity);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<T>> GetAllAsync(bool isNoTracking = false, CancellationToken cancellationToken = default)
        {
            var query = GetDbSet();

            if (isNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Set<T>().Update(entity);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<T>().AddRangeAsync(entities, cancellationToken);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Set<T>().UpdateRange(entities);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await DbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);

            DbContext.Set<T>().RemoveRange(entities);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> GetDbSet() => DbSet;

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().AsNoTracking().CountAsync(predicate, cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<T> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Set<T>().RemoveRange(entities);

            if (autoSave)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? DbSet.Where(predicate) : DbSet;
        }
    }
}
