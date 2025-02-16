using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class ShopController : Controller
    {
        IUnitOfWork _unitOfWork;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Products.GetAll("Category").ToList();
            List<ProductDisplay> productDisplays;

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                productDisplays = products.Select(p => new ProductDisplay{
                    SellPrice=p.SellPrice,
                    Stock = p.Stock,
                    CostPrice=p.CostPrice,
                    Discount=p.Discount,
                    ImageUrl=p.ImageUrl,
                    ProductId=p.ProductId,
                    Name=p.Name,
                    //AverageRating=,
                }).ToList();
                return View(productDisplays);
            }
            productDisplays = products.Select(p => new ProductDisplay
            {
                SellPrice = p.SellPrice,
                Stock = p.Stock,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                ImageUrl = p.ImageUrl,
                ProductId = p.ProductId,
                Name = p.Name,
                IsInCart = !(_unitOfWork.CartItems.Get(ci => ci.UserId == userId && ci.ProductId == p.ProductId)==null),
                IsInWishlist = !(_unitOfWork.WishlistItems.Get(ci => ci.UserId == userId && ci.ProductId == p.ProductId)==null)
            }).ToList();
            return View(productDisplays);
        }
        public IActionResult ProductDetail(int productId)
        {
            return View();
        }
    }
}