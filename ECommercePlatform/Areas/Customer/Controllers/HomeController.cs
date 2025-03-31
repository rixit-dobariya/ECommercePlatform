using System.Diagnostics;
using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork; 


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _unitOfWork.Products.GetAll("Category").ToListAsync();
            IEnumerable<ProductDisplay> productDisplays = await GetProductDisplays(products);

            HomeVM homeVM = new HomeVM
            {
                NewArrivals = productDisplays.OrderByDescending(p => p.ProductId).Take(10).ToList(),
                BestSellers = productDisplays.OrderByDescending(p => GetTotalQuantitySold(p.ProductId)).Take(10).ToList(),
                SaleItems = productDisplays.OrderByDescending(p => p.Discount).Take(10).ToList()
            };

            return View(homeVM);
        }

        private int GetTotalQuantitySold(int productId)
        {
            return _unitOfWork.OrderDetails
                .GetAll()
                .Where(od => od.ProductId == productId)
                .Sum(od => od.Quantity);
        }
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
        public async Task<IActionResult> About()
        {
            AboutPageContent aboutPageContent= await _unitOfWork.AboutPageContent.Get(a => a.Id == 1);
            return View(aboutPageContent);
        }
        public async Task<IActionResult> Contact()
        {
            ContactDetails contactDetails = await _unitOfWork.ContactDetails.Get(c => c.Id == 1);
            return View(new ContactVM { 
                PhoneNumbers = contactDetails.PhoneNumbers.Split(","),
                Emails = contactDetails.Emails.Split(","),
                Address = contactDetails.Address
            });
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Response response)
        {
            if (!ModelState.IsValid)
            {
                return View(new ContactVM { Response = response });
            }
            else
            {
                _unitOfWork.Responses.Add(response);
                await _unitOfWork.Save();
                TempData["success"] = "Your response has been stored successfully!";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
