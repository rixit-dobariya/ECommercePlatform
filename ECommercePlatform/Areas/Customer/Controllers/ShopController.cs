using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

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

            string lowerQuery = query.ToLower();

            var products = await _unitOfWork.Products.GetAll("Category")
                .Where(p => p.Name.ToLower().Contains(lowerQuery) ||
                            p.ShortDescription.ToLower().Contains(lowerQuery) ||
                            p.LongDescription.ToLower().Contains(lowerQuery) ||
                            (p.Category != null && p.Category.Name.ToLower().Contains(lowerQuery)))
                .ToListAsync();
            var categories = _unitOfWork.Categories.GetAll().Distinct().ToList();
            List<string> categoryNames = new List<string>();
            foreach (Category c in categories)
            {
                categoryNames.Add(c.Name);
            }
            ViewData["Categories"] = categoryNames;
            var productDisplays = await GetProductDisplays(products);
            return View("Index",productDisplays);
        }
        public async Task<IActionResult> Index(string query, string sort, string category, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 12)
        {
            // Prepare categories for dropdown
            var categories = _unitOfWork.Categories.GetAll().Distinct().ToList();
            List<string> categoryNames = categories.Select(c => c.Name).ToList();
            ViewData["Categories"] = categoryNames;

            // Start with all products
            var products = await _unitOfWork.Products.GetAll("Category").ToListAsync();

            // Search filter
            if (!string.IsNullOrWhiteSpace(query))
            {
                string lowerQuery = query.ToLower();
                products = products
                    .Where(p =>
                        p.Name.ToLower().Contains(lowerQuery) ||
                        p.ShortDescription.ToLower().Contains(lowerQuery) ||
                        p.LongDescription.ToLower().Contains(lowerQuery) ||
                        (p.Category != null && p.Category.Name.ToLower().Contains(lowerQuery))
                    ).ToList();
            }

            // Convert to display models
            var productDisplays = await GetProductDisplays(products);

            // Category filter
            if (!string.IsNullOrEmpty(category))
                productDisplays = productDisplays.Where(p => p.CategoryName == category);

            // Price filters (after discount)
            if (minPrice.HasValue)
                productDisplays = productDisplays.Where(p => p.SellPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                productDisplays = productDisplays.Where(p => p.SellPrice <= maxPrice.Value);

            // Sorting
            productDisplays = sort switch
            {
                "az" => productDisplays.OrderBy(p => p.Name),
                "za" => productDisplays.OrderByDescending(p => p.Name),
                "lowhigh" => productDisplays.OrderBy(p => p.SellPrice),
                "highlow" => productDisplays.OrderByDescending(p => p.SellPrice),
                _ => productDisplays.OrderBy(p => p.ProductId),
            };

            // Pagination
            int totalItems = productDisplays.Count();
            var paginatedProducts = productDisplays
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Query = query;
            ViewBag.Sort = sort;
            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(paginatedProducts);
        }


        public async Task<IActionResult> ProductDetails(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetAll("User")
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            User admin = await _unitOfWork.Users.Get(u=> u.Role == RoleType.Admin);
            ProductDetailsVM productDetailsVM = new()
            {
                Product = await _unitOfWork.Products.Get(p => p.ProductId == productId, "Category"),
                Reviews = reviews,
                CartQuantity = 0,
                IsInWishlist = false,
                HasOrdered = false,
                AverageRating = reviews.Count != 0 ? reviews.Average(r => r.Rating) : 0,
                AdminName = admin.FullName,
                AdminProfilePicture = admin.ProfilePicture
            };

            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId != null)
            {
                productDetailsVM.UserId = (int)userId;
                CartItem cartItem= await _unitOfWork.CartItems.Get(ci => ci.ProductId == productId && ci.UserId == userId);
                productDetailsVM.CartQuantity = cartItem == null ? 0 : cartItem.Quantity;
                productDetailsVM.IsInWishlist = await _unitOfWork.WishlistItems.Get(wi => wi.ProductId == productId && wi.UserId==userId) != null;
                productDetailsVM.HasOrdered = _unitOfWork.OrderHeaders.GetAll("OrderDetails").Any(oh => oh.UserId == userId && oh.OrderDetails.Any(od => od.ProductId == productId));
                productDetailsVM.Reviews = await _unitOfWork.Reviews.GetAll("User").OrderByDescending(r=>r.ReviewId).ToListAsync();
                Review? review = productDetailsVM.Reviews.Find(r => r.ProductId == productId && r.UserId == userId);
                if (review != null)
                {
                    productDetailsVM.Review = review;
                    if (productDetailsVM.Reviews.Remove(review))
                    {
                        productDetailsVM.Reviews.Insert(0, review);
                    }
                }
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
