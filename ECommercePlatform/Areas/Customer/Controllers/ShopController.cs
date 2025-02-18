using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("Index", new List<ProductDisplay>()); // Return an empty list if query is null
            }

            IEnumerable<Product> products = _unitOfWork.Products.GetAll("Category")
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) || p.ShortDescription.ToLower().Contains(query.ToLower()) || p.LongDescription.ToLower().Contains(query.ToLower()) || p.Category.Name.ToLower().Contains(query.ToLower()));

            List<ProductDisplay> productDisplays;

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                productDisplays = products.Select(p => new ProductDisplay
                {
                    SellPrice = p.SellPrice,
                    Stock = p.Stock,
                    CostPrice = p.CostPrice,
                    Discount = p.Discount,
                    ImageUrl = p.ImageUrl,
                    ProductId = p.ProductId,
                    Name = p.Name,
                    AverageRating = 3,
                    CategoryName = p.Category.Name,
                    reviewCount = _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == p.ProductId).Count(),
                    ShortDescription = p.ShortDescription,
                }).ToList();
                return View("Index", productDisplays);
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
                AverageRating = 3,
                CategoryName = p.Category.Name,
                reviewCount = _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == p.ProductId).Count(),
                ShortDescription = p.ShortDescription,
                CartQuantity = _unitOfWork.CartItems.Get(ci => ci.ProductId == p.ProductId && ci.UserId == userId)?.Quantity ?? 0,
                IsInWishlist = _unitOfWork.WishlistItems.Get(wi => wi.ProductId == p.ProductId && wi.UserId == userId) != null
            }).ToList();
            return View("Index",productDisplays);
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAll("Category").ToList();
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
                    AverageRating=3,
                    CategoryName = p.Category.Name,
                    reviewCount = _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == p.ProductId).Count(),
                    ShortDescription = p.ShortDescription,
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
                AverageRating = 3,
                CategoryName = p.Category.Name,
                reviewCount = _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == p.ProductId).Count(),
                ShortDescription = p.ShortDescription,
                CartQuantity = _unitOfWork.CartItems.Get(ci => ci.ProductId == p.ProductId && ci.UserId == userId)?.Quantity??0,
                IsInWishlist = _unitOfWork.WishlistItems.Get(wi => wi.ProductId == p.ProductId && wi.UserId == userId)!=null
            }).ToList();
            return View(productDisplays);
        }
        public IActionResult ProductDetails(int productId)
        {
            ProductDetailsVM productDetailsVM = new()
            {
                Product = _unitOfWork.Products.Get(p => p.ProductId == productId, "Category"),
                Reviews = _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == productId).ToList(),
                CartQuantity = 0,
                IsInWishlist = false,
                HasOrdered = false
            };
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId != null)
            {
                CartItem cartItem= _unitOfWork.CartItems.Get(ci => ci.ProductId == productId && ci.UserId == userId);
                productDetailsVM.CartQuantity = cartItem == null ? 0 : cartItem.Quantity;
                productDetailsVM.IsInWishlist = _unitOfWork.WishlistItems.Get(wi => wi.ProductId == productId && wi.UserId==userId) != null;
                productDetailsVM.HasOrdered = _unitOfWork.OrderHeaders.GetAll("OrderDetails").Any(oh => oh.UserId == userId && oh.OrderDetails.Any(od => od.ProductId == productId));
            }
            return View(productDetailsVM);
        }
    }
}