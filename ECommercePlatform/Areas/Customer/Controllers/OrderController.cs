using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class OrderController : Controller
    {
        IUnitOfWork _unitOfWork {  get; set; }
        public OrderController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Display(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login","User");
            }
            if (orderId <= 0)
            {
                return NotFound();
            }
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.Get(oh=>oh.OrderId==orderId, "User,ShippingAddress,OrderDetails");
            if (orderHeader.UserId != userId)
            {
                TempData["error"] = "You are not allowed to see this order";
                return RedirectToAction("History", "Order");
            }
            foreach (var orderDetail in orderHeader.OrderDetails)
            {
                orderDetail.Product = _unitOfWork.Products.Get(p => p.ProductId == orderDetail.ProductId);
            }

            return View(orderHeader);
        }

        public IActionResult History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to view orders";
                return RedirectToAction("Login", "User");
            }
            IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaders.GetAll().Where(oh => oh.UserId == userId);
            return View(orderHeaders);
        }
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to access this page";
                return RedirectToAction("Login", "User");
            }
            CheckoutVM checkoutVM = GetCheckoutVM(userId);
            if (checkoutVM.CartItems.Count() == 0)
            {
                TempData["error"] = "Your cart is empty, so add products to order them";
                return RedirectToAction("Index", "Shop");
            }
            return View(checkoutVM);
        }
        [HttpPost]
        public IActionResult AddAddress(Address address)
        {
            if (ModelState.IsValid)
            {
                address.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (address.UserId > 0)
                {
                    _unitOfWork.Addresses.Add(address);
                    _unitOfWork.Save();
                    TempData["success"] = "Address added successfully.";

                }
                else
                {
                    TempData["error"] = "You need to login first to add address";
                    return RedirectToAction("Login","User");
                }
                return RedirectPermanent("Checkout");
            }
            return Redirect("Checkout");
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutVM checkoutVM)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["error"] = "You must be logged in to place order.";
                return RedirectToAction("Login","User");
            }

            CheckoutVM fetchedCheckoutVM = GetCheckoutVM(userId);

            if (checkoutVM.ShippingAddressId <= 0)
            {
                ModelState.AddModelError("ShippingAddressId","You must select address to place order.");
                return View(fetchedCheckoutVM);
            }

            fetchedCheckoutVM.ShippingAddressId = checkoutVM.ShippingAddressId;
            checkoutVM = fetchedCheckoutVM;

            OrderHeader orderHeader = new()
            {
                UserId = (int)userId,
                OrderStatus= OrderStatus.Pending,
                ShippingAddressId = checkoutVM.ShippingAddressId,
                ShippingCharge = checkoutVM.ShippingCharge,
                Subtotal = checkoutVM.Total - checkoutVM.ShippingCharge
            };
            //make used address as deleted 
            Address address = _unitOfWork.Addresses.Get(a => a.AddressId == checkoutVM.ShippingAddressId);
            address.IsDeleted = 1;
            _unitOfWork.Addresses.Update(address);
            address.AddressId = 0;
            _unitOfWork.Addresses.Add(address);

            _unitOfWork.OrderHeaders.Add(orderHeader);
            _unitOfWork.Save();
            List<OrderDetail>  orderDetails = new List<OrderDetail>();
            foreach(var cartItem in checkoutVM.CartItems)
            {
                OrderDetail orderDetail = new()
                {
                    OrderHeaderId = orderHeader.OrderId,
                    ProductId = cartItem.ProductId,
                    DiscountAmount = 1,
                    Price = cartItem.Product.SellPrice - cartItem.Product.SellPrice * cartItem.Product.Discount / 100,
                    Quantity = cartItem.Quantity,
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
            }
            _unitOfWork.CartItems.RemoveRange(checkoutVM.CartItems);
            _unitOfWork.Save();
            TempData["success"] = "Order placed successfully.";
            return RedirectToAction("History");
        }
        #region METHODS
        CheckoutVM GetCheckoutVM(int? userId)
        {
            CheckoutVM checkoutVM = new()
            {
                CartItems = _unitOfWork.CartItems.GetAll("Product").Where(ci => ci.UserId == userId),
                Addresses = _unitOfWork.Addresses.GetAll().Where(a => a.UserId == userId),
            };
            checkoutVM.Total = checkoutVM.CartItems
                    .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                    .DefaultIfEmpty(0)
                    .Sum();
            checkoutVM.ShippingCharge = checkoutVM.Total >= 500 ? 0 : 50;
            checkoutVM.Total += checkoutVM.ShippingCharge;
            return checkoutVM;
        }
        #endregion
    }
}
