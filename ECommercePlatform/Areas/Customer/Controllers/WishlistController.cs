using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(IEnumerable<WishlistItem> wishlistItems)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            wishlistItems = _unitOfWork.WishlistItems.GetAll("Product")
                .Where(ci => ci.UserId == Convert.ToInt32(userId));
            return View(wishlistItems);
        }
        public IActionResult Add(int productId, int quantity = 1)
        {
            if (productId == 0)
            {
                return RedirectToActionPermanent("Index");
            }
            string? userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            WishlistItem checkWishlistItem = _unitOfWork.WishlistItems.Get(ci => productId == ci.ProductId && ci.UserId == Convert.ToInt32(userId));
            if (checkWishlistItem != null)
            {
                TempData["error"] = "Product is already available in wishlist!";
                return RedirectToActionPermanent("Index");
            }
            //add product to wishlist
            WishlistItem wishlistItem = new()
            {
                ProductId = productId,
                UserId = Convert.ToInt32(userId)
            };
            _unitOfWork.WishlistItems.Add(wishlistItem);
            _unitOfWork.Save();
            TempData["success"] = "Product added to wishlist successfully!";
            return RedirectToActionPermanent("Index");
        }
        public IActionResult Remove(int productId)
        {
            string? userId = HttpContext.Session.GetString("UserId");
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
                WishlistItem wishlistItem = _unitOfWork.WishlistItems.Get(ci => ci.ProductId == productId && ci.UserId == Convert.ToInt32(userId));
                _unitOfWork.WishlistItems.Remove(wishlistItem);
                _unitOfWork.Save();
                TempData["success"] = "Product removed from wishlist successfully!";
            }
            return RedirectToActionPermanent("Index");
        }
    }
}
