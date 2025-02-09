using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;

namespace ECommercePlatform.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Category> Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IRepository<ProductAttribute> ProductAttributes { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRepository<Offer> Offers { get; private set; }
        public IRepository<UserOTP> UserOTPs { get; private set; }
        public IRepository<CartItem> CartItems { get; private set; }
        public IRepository<WishlistItem> WishlistItems { get; private set; }
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new Repository<Category>(db);
            Products = new ProductRepository(db);
            ProductAttributes = new Repository<ProductAttribute>(db);
            Users = new UserRepository(db);
            Offers = new Repository<Offer>(db);
            UserOTPs = new Repository<UserOTP>(db);
            CartItems = new Repository<CartItem>(db);
            WishlistItems = new Repository<WishlistItem>(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
