using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;

namespace ECommercePlatform.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Product> Products { get; private set; }
        public IRepository<ProductAttribute> ProductAttributes { get; private set; }

        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new Repository<Category>(db);
            Products = new Repository<Product>(db);
            ProductAttributes = new Repository<ProductAttribute>(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
