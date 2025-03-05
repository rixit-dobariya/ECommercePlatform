using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<Category> Categories {  get; }
        IProductRepository Products { get; }
        IRepository<ProductAttribute> ProductAttributes { get; }
        IUserRepository Users { get; }
        IRepository<Offer> Offers { get; }
        IRepository<UserOTP> UserOTPs { get; }
        IRepository<CartItem> CartItems { get;}
        IRepository<WishlistItem> WishlistItems { get;}
        IRepository<Address> Addresses { get;}
        IRepository<OrderHeader> OrderHeaders { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        IRepository<Review> Reviews { get; }
        IRepository<Response> Responses { get; }

        Task Save();
    }
}
