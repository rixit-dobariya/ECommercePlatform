using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;

namespace ECommercePlatform.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public override IEnumerable<Product> GetAll(string? includeProperties = null)
        {
            return base.GetAll(includeProperties).Where(p => p.IsActive == 1);
        }

        public IEnumerable<Product> GetAllDeletedProducts(string? includeProperties = null)
        {
            return base.GetAll(includeProperties).Where(p => p.IsActive == 0);

        }
    }
}
