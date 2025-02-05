using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllDeletedProducts(string? includeProperties = null);
    }
}
