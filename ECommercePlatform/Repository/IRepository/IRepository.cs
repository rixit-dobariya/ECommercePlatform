using System.Linq.Expressions;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll(string? includeProperties = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
