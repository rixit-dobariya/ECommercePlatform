using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetAllDeletedProducts(string? includeProperties = null);
    }
}
