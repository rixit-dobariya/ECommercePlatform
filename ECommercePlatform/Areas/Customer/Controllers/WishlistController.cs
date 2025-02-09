using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    public class WishlistController : Controller
    {
        IUnitOfWork _unitOfWork { get; set; }
        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add(int productId)
        {
            if (productId == null || productId == 0)
            {
                return RedirectToActionPermanent("Index");
            }
            string userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            WishlistItem wishlistItem = new()
            {
                ProductId = productId,
                UserId = Convert.ToInt32(userId)
            };
            _unitOfWork.WishlistItems.Add(wishlistItem);
            return RedirectToActionPermanent("Index");
        }
    }
}
