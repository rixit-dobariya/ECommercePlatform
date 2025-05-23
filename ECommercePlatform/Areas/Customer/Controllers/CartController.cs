using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    [AuthCheck]
    public class CartController : Controller
    {
        IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            CartVM cartVM = new();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Shop", "Index"); // or some login redirect

            cartVM.CartItems = await _unitOfWork.CartItems.GetAll("Product")
                .Where(ci => ci.UserId == userId.Value).ToListAsync();

            decimal subTotal = cartVM.CartItems
                .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                .DefaultIfEmpty(0)
                .Sum();

            decimal discount = Decimal.Parse(HttpContext.Session.GetString("Discount") ?? "0") ;

            cartVM.Total = subTotal - discount;
            cartVM.ShippingCharge = cartVM.Total >= 500 ? 0 : 50;
            cartVM.Discount = discount; // Add this property in your CartVM

            return View(cartVM);
        }
        public async Task<IActionResult> Add(int productId, int quantity=1)
        {
            if(productId == 0)
            {
                return RedirectToAction("Index");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            CartItem checkCartItem = await _unitOfWork.CartItems.Get(ci => productId == ci.ProductId && ci.UserId == Convert.ToInt32(userId));
            if (checkCartItem != null)
            {
                TempData["error"] = "Product is already available in cart!";
                return RedirectToAction("Index");
            }
            //add product to cart
            CartItem cartItem = new()
            {
                ProductId = productId,
                Quantity = quantity,
                UserId = Convert.ToInt32(userId)
            };
            _unitOfWork.CartItems.Add(cartItem);
            await _unitOfWork.Save();
            TempData["success"] = "Product added to cart successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(CartItem cartItem)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                CartItem newCartItem = await _unitOfWork.CartItems.Get(ci => ci.UserId == Convert.ToInt32(userId) && ci.ProductId == cartItem.ProductId);
                newCartItem.Quantity = cartItem.Quantity;
                _unitOfWork.CartItems.Update(newCartItem);
                await _unitOfWork.Save();
                TempData["success"] = "Cart quantity updated successfully!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Cart quantity update failed!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(productId == 0)
            {
                TempData["error"] = "Deletion failed";
            }
            else
            {
                CartItem cartItem = await _unitOfWork.CartItems.Get(ci => ci.ProductId == productId && ci.UserId == userId);
                _unitOfWork.CartItems.Remove(cartItem);
                await _unitOfWork.Save();
                TempData["success"] = "Product removed from cart successfully!";
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Clear()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            IEnumerable<CartItem> cartItems = await _unitOfWork.CartItems.GetAll().Where(c => c.UserId == userId).ToListAsync();
            _unitOfWork.CartItems.RemoveRange(cartItems);
            await _unitOfWork.Save();
            TempData["success"] = "Cart cleared successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            string couponCodeLower = couponCode.ToLower();

            Offer offer = await _unitOfWork.Offers.Get(o =>
                o.OfferCode.ToLower() == couponCodeLower && o.StartDate <= today && today <= o.EndDate);

            if (offer == null)
            {
                TempData["CouponMessage"] = "Invalid or expired promo code.";
                HttpContext.Session.Remove("Discount");
                return RedirectToAction("Index");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["CouponMessage"] = "User not logged in.";
                return RedirectToAction("Index");
            }

            var cartItems = await _unitOfWork.CartItems.GetAll("Product")
                .Where(ci => ci.UserId == userId.Value).ToListAsync();

            decimal total = cartItems
                .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                .DefaultIfEmpty(0)
                .Sum();

            if (total < offer.MinimumAmount)
            {
                TempData["CouponMessage"] = $"Cart total must be at least ₹{offer.MinimumAmount} to apply this promo.";
                HttpContext.Session.Remove("Discount");
                return RedirectToAction("Index");
            }

            // Calculate the discount amount
            decimal discountAmount = (total * offer.Discount) / 100;

            // Apply max discount limit
            if (discountAmount > offer.MaxDiscountAmount)
            {
                discountAmount = offer.MaxDiscountAmount;
            }

            // Save the flat discount amount in session (store as int cents or decimal as needed)
            // Here storing as decimal by serializing as string since session only stores strings/byte[]
            HttpContext.Session.SetString("Discount", discountAmount.ToString("F2")); // 2 decimal places

            TempData["CouponMessage"] = $"Promo code applied! You get ₹{discountAmount:F2} off.";

            return RedirectToAction("Index");
        }

    }
}
