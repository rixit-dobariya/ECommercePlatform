using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAll(string? includeProperties = null);
        IEnumerable<Product> GetAllDeletedProducts(string? includeProperties = null);
    }
}
