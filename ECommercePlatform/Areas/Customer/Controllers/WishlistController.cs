using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class WishlistController : Controller
    {
        IUnitOfWork _unitOfWork;
        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(IEnumerable<WishlistItem> wishlistItems)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            wishlistItems = await _unitOfWork.WishlistItems.GetAll("Product")
                .Where(ci => ci.UserId == Convert.ToInt32(userId)).ToListAsync();
            return View(wishlistItems);
        }
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            if (productId == 0)
            {
                return RedirectToAction("Index");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            WishlistItem checkWishlistItem = await _unitOfWork.WishlistItems.Get(ci => productId == ci.ProductId && ci.UserId == Convert.ToInt32(userId));
            if (checkWishlistItem != null)
            {
                TempData["error"] = "Product is already available in wishlist!";
                return RedirectToAction("Index");
            }
            //add product to wishlist
            WishlistItem wishlistItem = new()
            {
                ProductId = productId,
                UserId = Convert.ToInt32(userId)
            };
            _unitOfWork.WishlistItems.Add(wishlistItem);
            await _unitOfWork.Save();
            TempData["success"] = "Product added to wishlist successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (productId == 0)
            {
                TempData["error"] = "Deletion failed";
            }
            else if (userId == null)
            {
                TempData["error"] = "Deletion failed";
            }
            else
            {
                WishlistItem wishlistItem = await _unitOfWork.WishlistItems.Get(ci => ci.ProductId == productId && ci.UserId == Convert.ToInt32(userId));
                _unitOfWork.WishlistItems.Remove(wishlistItem);
                await _unitOfWork.Save();
                TempData["success"] = "Product removed from wishlist successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
