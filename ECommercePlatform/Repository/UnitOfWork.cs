using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;

namespace ECommercePlatform.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //public ICategoryRepository CategoryRepository { get; private set; }
        //public IProductRepository ProductRepository { get; private set; }


        public IRepository<Category> Categories { get; private set; }
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new Repository<Category>(db);


            //CategoryRepository = new CategoryRepository(_db);
            //ProductRepository = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
