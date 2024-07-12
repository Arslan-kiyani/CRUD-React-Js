using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using System.Linq.Expressions;

namespace ReadFile_Mini.Models.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SeniorDb _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(SeniorDb context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual async Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ?
               await orderBy(query).ToListAsync() :
               await query.ToListAsync();
        }
        public virtual async Task<TEntity> GetSingleByFilter(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            var res = await _dbSet.AddAsync(entity);
            return res.Entity;
        }
        public virtual async Task<int> Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
