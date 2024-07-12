using System.Linq.Expressions;

namespace ReadFile_Mini.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        Task<TEntity> GetSingleByFilter(Expression<Func<TEntity, bool>> filter, string includeProperties = "");

        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<TEntity> Insert(TEntity entity);
        Task<int> Update(TEntity entity);
        Task<bool> SaveChanges();

    }
}
