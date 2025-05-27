using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Helpers.PaymentHelper;
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
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(IUnitOfWork unitOfWork, IPaymentService paymentService, IHttpContextAccessor httpContextAccessor) 
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Display(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (orderId <= 0)
            {
                return NotFound();
            }
            OrderHeader orderHeader = await _unitOfWork.OrderHeaders.Get(oh=>oh.OrderId==orderId, "User,ShippingAddress,OrderDetails");
            if (orderHeader.UserId != userId)
            {
                TempData["error"] = "You are not allowed to see this order";
                return RedirectToAction("History", "Order");
            }
            foreach (var orderDetail in orderHeader.OrderDetails)
            {
                orderDetail.Product = await _unitOfWork.Products.Get(p => p.ProductId == orderDetail.ProductId);
            }
            decimal actualSubtotal = orderHeader.OrderDetails
    .Select(od => (od.Product.SellPrice - (od.Product.SellPrice * od.Product.Discount / 100)) * od.Quantity)
    .DefaultIfEmpty(0)
    .Sum();

            decimal discountAmount = Math.Round(actualSubtotal - orderHeader.Subtotal, 2);
            ViewBag.DiscountAmount = discountAmount;

            return View(orderHeader);
        }

        public async Task<IActionResult> History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["error"] = "You must be logged in to view orders";
                return RedirectToAction("Login", "User");
            }
            IEnumerable<OrderHeader> orderHeaders = await _unitOfWork.OrderHeaders.GetAll().Where(oh => oh.UserId == userId).ToListAsync();
            return View(orderHeaders);
        }
        public async Task<IActionResult> Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            CheckoutVM checkoutVM = await GetCheckoutVM(userId);
            if (checkoutVM.CartItems.Count() == 0)
            {
                TempData["error"] = "Your cart is empty, so add products to order them";
                return RedirectToAction("Index", "Shop");
            }
            return View(checkoutVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(Address address)
        {
            if (ModelState.IsValid)
            {
                address.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (address.UserId > 0)
                {
                    _unitOfWork.Addresses.Add(address);
                    await _unitOfWork.Save();
                    TempData["success"] = "Address added successfully.";
                }
                else
                {
                    TempData["error"] = "You need to login first to add address";
                    return RedirectToAction("Login","User");
                }
                return RedirectToAction("Checkout");
            }
            return Redirect("Checkout");
        }
        public async Task CompleteOrder()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["error"] = "User not logged in.";
                return;
            }

            CheckoutVM fetchedCheckoutVM = await GetCheckoutVM(userId);
            int? shippingAddressId = HttpContext.Session.GetInt32("ShippingAddressId");

            if (shippingAddressId == null)
            {
                TempData["error"] = "Shipping address not found.";
                return;
            }

            // Check stock availability
            foreach (var cartItem in fetchedCheckoutVM.CartItems)
            {
                var product = await _unitOfWork.Products.Get(p => p.ProductId == cartItem.ProductId);
                if (product == null || product.Stock < cartItem.Quantity)
                {
                    TempData["error"] = $"Stock not available for product '{cartItem.Product?.Name ?? "Unknown"}'.";
                    return;
                }
            }

            fetchedCheckoutVM.ShippingAddressId = (int)shippingAddressId;

            OrderHeader orderHeader = new()
            {
                UserId = (int)userId,
                OrderStatus = OrderStatus.Pending,
                ShippingAddressId = fetchedCheckoutVM.ShippingAddressId,
                ShippingCharge = fetchedCheckoutVM.ShippingCharge,
                Subtotal = fetchedCheckoutVM.Total - fetchedCheckoutVM.ShippingCharge
            };

            // Mark address as used (soft-delete) and clone it
            Address address = await _unitOfWork.Addresses.Get(a => a.AddressId == shippingAddressId);
            address.IsDeleted = 1;
            _unitOfWork.Addresses.Update(address);

            address.AddressId = 0;
            _unitOfWork.Addresses.Add(address);

            _unitOfWork.OrderHeaders.Add(orderHeader);
            await _unitOfWork.Save();

            foreach (var cartItem in fetchedCheckoutVM.CartItems)
            {
                var product = await _unitOfWork.Products.Get(p => p.ProductId == cartItem.ProductId);

                OrderDetail orderDetail = new()
                {
                    OrderHeaderId = orderHeader.OrderId,
                    ProductId = cartItem.ProductId,
                    DiscountAmount = 1,
                    Price = cartItem.Product.SellPrice - cartItem.Product.SellPrice * cartItem.Product.Discount / 100,
                    Quantity = cartItem.Quantity,
                };

                _unitOfWork.OrderDetails.Add(orderDetail);

                // Update product stock
                product.Stock -= cartItem.Quantity;
                _unitOfWork.Products.Update(product);
            }

            // Clear cart
            _unitOfWork.CartItems.RemoveRange(fetchedCheckoutVM.CartItems);
            await _unitOfWork.Save();

            TempData["success"] = "Order placed successfully.";
        }
        #region METHODS
        public async Task<CheckoutVM> GetCheckoutVM(int? userId)
        {
            CheckoutVM checkoutVM = new()
            {
                CartItems = await _unitOfWork.CartItems.GetAll("Product").Where(ci => ci.UserId == userId).ToListAsync(),
                Addresses = await _unitOfWork.Addresses.GetAll().Where(a => a.UserId == userId).ToListAsync(),
            };

            // Calculate total before shipping and discount
            checkoutVM.Total = checkoutVM.CartItems
                .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                .DefaultIfEmpty(0)
                .Sum();

            // Retrieve discount from session and convert to decimal
            string discountString = _httpContextAccessor.HttpContext?.Session.GetString("Discount");
            decimal discount = 0;

            if (!string.IsNullOrEmpty(discountString) && decimal.TryParse(discountString, out decimal parsedDiscount))
            {
                discount = parsedDiscount;
            }

            checkoutVM.Discount = discount;

            // Apply shipping charge
            checkoutVM.ShippingCharge = checkoutVM.Total >= 500 ? 0 : 50;

            // Apply discount to total before adding shipping
            checkoutVM.Total = checkoutVM.Total - discount + checkoutVM.ShippingCharge;

            return checkoutVM;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> ProcessOrderRequest(CheckoutVM checkoutVM)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["error"] = "You must be logged in to place an order.";
                    return RedirectToAction("Login", "User");
                }

                CheckoutVM fetchedCheckoutVM = await GetCheckoutVM(userId);

                if (checkoutVM.ShippingAddressId <= 0)
                {
                    ModelState.AddModelError("ShippingAddressId", "You must select an address to place an order.");
                    return View(fetchedCheckoutVM);
                }

                // ✅ Validate stock availability before proceeding
                foreach (var cartItem in fetchedCheckoutVM.CartItems)
                {
                    Product? product = await _unitOfWork.Products.Get(p => p.ProductId == cartItem.ProductId);

                    if (product == null)
                    {
                        TempData["error"] = $"Product with ID {cartItem.ProductId} not found.";
                        return RedirectToAction("Index", "Cart");
                    }

                    if (product.Stock < cartItem.Quantity)
                    {
                        TempData["error"] = $"Stock is not available for product '{product.Name}'. Only {product.Stock} left.";
                        return RedirectToAction("Index", "Cart");
                    }
                }

                HttpContext.Session.SetInt32("ShippingAddressId", checkoutVM.ShippingAddressId);

                User user = await _unitOfWork.Users.Get(u => u.UserId == userId);
                Address address = await _unitOfWork.Addresses.Get(a => a.AddressId == checkoutVM.ShippingAddressId);

                PaymentRequest paymentRequest = new PaymentRequest()
                {
                    Address = $"{address.FirstName} {address.LastName} \n {address.Region}\n{address.City}, {address.State}\nPin Code:{address.PinCode}\nPhone:{address.Phone}",
                    Amount = fetchedCheckoutVM.Total,
                    Email = user.Email,
                    Name = user.FullName,
                    PhoneNumber = user.Phone
                };

                MerchantOrder _merchantOrder = await _paymentService.ProcessMerchantOrder(paymentRequest);
                return View("Payment", _merchantOrder);
            }
            catch (Exception ex)
            {
                // Log the exception
                return RedirectToAction("Failed");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess()
        {
            try
            {
                string paymentMessage = await _paymentService.CompleteOrderProcess(_httpContextAccessor);
                if(paymentMessage == "captured")
                {
                    await CompleteOrder();
                    return RedirectToAction("History");
                }
                else
                {
                    TempData["error"] = "Order failed";
                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return RedirectToAction("Failed");
            }
        }

    }
}
