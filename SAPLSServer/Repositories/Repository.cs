using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey> where T : class where TKey : class
    {
        protected readonly SaplsContext _context;
        protected readonly DbSet<T> _dbSet;

        protected Repository(SaplsContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>[]? filters = null, Expression<Func<T, object>>? sortBy = null, bool isAscending = true)
        {
            var query = ApplyFilters(_dbSet.AsQueryable(), filters ?? Array.Empty<Expression<Func<T, bool>>>());
            if (sortBy != null)
            {
                query = isAscending ? query.OrderBy(sortBy) : query.OrderByDescending(sortBy);
            }
            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetPageAsync(int pageNumber = 1, int pageSize = 20, Expression<Func<T, bool>>[]? filters = null, Expression<Func<T, object>>? sortBy = null, bool isAscending = true)
        {
            var query = ApplyFilters(_dbSet.AsQueryable(), filters ?? Array.Empty<Expression<Func<T, bool>>>());
            if (sortBy != null)
            {
                query = isAscending ? query.OrderBy(sortBy) : query.OrderByDescending(sortBy);
            }

            return await query.Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public virtual async Task<T?> Find(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T?> Find(Expression<Func<T, bool>>[] criterias)
        {
            if (criterias == null || criterias.Length == 0)
                return null;

            var query = _dbSet.AsQueryable();

            foreach (var criteria in criterias.Where(c => c != null))
            {
                query = query.Where(criteria);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        private static IQueryable<T> ApplyFilters(IQueryable<T> query, params Expression<Func<T, bool>>[] filters)
        {
            foreach (var filter in filters.Where(f => f != null))
            {
                query = query.Where(filter);
            }
            return query;
        }
    }
}
