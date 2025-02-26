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
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("Index", new List<ProductDisplay>()); // Return an empty list if query is null
            }

            IEnumerable<Product> products = await _unitOfWork.Products.GetAll("Category")
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) || p.ShortDescription.ToLower().Contains(query.ToLower()) || p.LongDescription.ToLower().Contains(query.ToLower()) || p.Category.Name.ToLower().Contains(query.ToLower())).ToListAsync();
            IEnumerable<ProductDisplay> productDisplays = await GetProductDisplays(products);
            return View("Index",productDisplays);
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _unitOfWork.Products.GetAll("Category").ToListAsync();
            IEnumerable<ProductDisplay> productDisplays = await GetProductDisplays(products);
            return View(productDisplays);
        }
        public async Task<IActionResult> ProductDetails(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetAll("User")
                .Where(r => r.ProductId == productId)
                .ToListAsync(); 

            ProductDetailsVM productDetailsVM = new()
            {
                Product = await _unitOfWork.Products.Get(p => p.ProductId == productId, "Category"),
                Reviews = reviews,
                CartQuantity = 0,
                IsInWishlist = false,
                HasOrdered = false,
                AverageRating = reviews.Count != 0 ? reviews.Average(r => r.Rating) : 0,
            };

            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId != null)
            {
                CartItem cartItem= await _unitOfWork.CartItems.Get(ci => ci.ProductId == productId && ci.UserId == userId);
                productDetailsVM.CartQuantity = cartItem == null ? 0 : cartItem.Quantity;
                productDetailsVM.IsInWishlist = _unitOfWork.WishlistItems.Get(wi => wi.ProductId == productId && wi.UserId==userId) != null;
                productDetailsVM.HasOrdered = _unitOfWork.OrderHeaders.GetAll("OrderDetails").Any(oh => oh.UserId == userId && oh.OrderDetails.Any(od => od.ProductId == productId));
                productDetailsVM.Reviews = _unitOfWork.Reviews.GetAll("User").ToList();
                productDetailsVM.Review = await _unitOfWork.Reviews.Get(r => r.ProductId == productId && r.UserId == userId) ?? new Review();
            }
            return View(productDetailsVM);
        }
        #region methods
        private async Task<IEnumerable<ProductDisplay>> GetProductDisplays(IEnumerable<Product> products)
        {
            IEnumerable<ProductDisplay> productDisplays;
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                productDisplays = products
                .Select(p => new ProductDisplay
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
                    ShortDescription = p.ShortDescription,
                }).ToList();
                foreach (var productDisplay in productDisplays)
                {
                    productDisplay.reviewCount = await _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == productDisplay.ProductId).CountAsync();
                }
                return productDisplays;
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
                ShortDescription = p.ShortDescription,
            }).ToList();
            foreach (var productDisplay in productDisplays)
            {
                productDisplay.reviewCount = await _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == productDisplay.ProductId).CountAsync();
                CartItem cartItem = await _unitOfWork.CartItems.Get(ci => ci.ProductId == productDisplay.ProductId && ci.UserId == (int)userId);
                cartItem = cartItem ?? new CartItem();
                cartItem.Quantity = cartItem.Quantity;
                WishlistItem wishlistItem = await _unitOfWork.WishlistItems.Get(wi => wi.ProductId == productDisplay.ProductId && wi.UserId == userId);
                productDisplay.IsInWishlist = wishlistItem != null;
            }
            return productDisplays;
        }
        #endregion
    }
}
