using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class CartController : Controller
    {
        IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            CartVM cartVM  = new();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            cartVM.CartItems = _unitOfWork.CartItems.GetAll("Product")
                .Where(ci => ci.UserId == Convert.ToInt32(userId));
            cartVM.Total = cartVM.CartItems
                    .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                    .DefaultIfEmpty(0)
                    .Sum();
            cartVM.ShippingCharge = cartVM.Total >= 500 ? 0 : 50;
            return View(cartVM);
        }
        public IActionResult Add(int productId, int quantity=1)
        {
            if(productId == 0)
            {
                return RedirectToActionPermanent("Index");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            CartItem checkCartItem = _unitOfWork.CartItems.Get(ci => productId == ci.ProductId && ci.UserId == Convert.ToInt32(userId));
            if (checkCartItem != null)
            {
                TempData["error"] = "Product is already available in cart!";
                return RedirectToActionPermanent("Index");
            }
            //add product to cart
            CartItem cartItem = new()
            {
                ProductId = productId,
                Quantity = quantity,
                UserId = Convert.ToInt32(userId)
            };
            _unitOfWork.CartItems.Add(cartItem);
            _unitOfWork.Save();
            TempData["success"] = "Product added to cart successfully!";
            return RedirectToActionPermanent("Index");
        }
        [HttpPost]
        public IActionResult Update(CartItem cartItem)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            if (ModelState.IsValid)
            {
                CartItem newCartItem = _unitOfWork.CartItems.Get(ci => ci.UserId == Convert.ToInt32(userId) && ci.ProductId == cartItem.ProductId);
                newCartItem.Quantity = cartItem.Quantity;
                _unitOfWork.CartItems.Update(newCartItem);
                _unitOfWork.Save();
                TempData["success"] = "Cart quantity updated successfully!";
                return RedirectToActionPermanent("Index");
            }
            TempData["error"] = "Cart quantity update failed!";
            return RedirectToActionPermanent("Index");
        }
        public IActionResult Remove(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(productId == 0)
            {
                TempData["error"] = "Deletion failed";
            }
            else if (userId == null)
            {
                TempData["error"] = "Deletion failed";
            }
            else
            {
                CartItem cartItem = _unitOfWork.CartItems.Get(ci => ci.ProductId == productId && ci.UserId == Convert.ToInt32(userId));
                _unitOfWork.CartItems.Remove(cartItem);
                _unitOfWork.Save();
                TempData["success"] = "Product removed from cart successfully!";
            }
            return RedirectToActionPermanent("Index");
        }
    }
}
