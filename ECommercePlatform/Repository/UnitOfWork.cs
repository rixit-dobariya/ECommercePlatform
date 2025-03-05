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
        public IRepository<Address> Addresses { get; private set; }
        public IRepository<OrderHeader> OrderHeaders { get; private set; }
        public IRepository<OrderDetail> OrderDetails { get; private set; }
        public IRepository<Review> Reviews { get; private set; }
        public IRepository<Response> Responses { get; private set; }

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
            Addresses = new Repository<Address>(db);
            OrderHeaders = new Repository<OrderHeader>(db);
            OrderDetails = new Repository<OrderDetail>(db);
            Reviews = new Repository<Review>(db);
            Responses = new Repository<Response>(db);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
