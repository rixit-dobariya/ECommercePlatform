using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class OrderController : Controller
    {
        IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Order";

            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeaders.Get(o => o.OrderId == id, "User,ShippingAddress,OrderDetails");
            foreach (var orderDetail in orderHeader.OrderDetails)
            {
                orderDetail.Product = await _unitOfWork.Products.Get(p => p.ProductId == orderDetail.ProductId);
            }
            return View(orderHeader);
        }
        #region API ENDPOINTS
        public async Task<JsonResult> GetAll()
        {
            IEnumerable<OrderHeader> orders = await _unitOfWork.OrderHeaders.GetAll("User").Where(o => !o.IsDeleted).ToListAsync();
            return Json(new
            {
                success = true,
                data = orders.Select(o => new{
                    o.OrderId, 
                    UserFullName=o.User.FullName,
                    o.ShippingCharge,
                    o.Subtotal, 
                    Total = o.Subtotal+o.ShippingCharge, 
                    OrderStatus = o.OrderStatus.ToString(), 
                    PaymentMode = o.PaymentMode.ToString(),
                    OrderDate =  o.OrderDate.Date.ToShortDateString()
                })
            });
        }
        public async Task<JsonResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No order found."
                });
            }
            OrderHeader orderHeader = await _unitOfWork.OrderHeaders.Get(o => o.OrderId == id);
            orderHeader.IsDeleted = true;
            _unitOfWork.OrderHeaders.Update(orderHeader);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Order Deleted Successfully!"
            });
        }
        #endregion
    }
}
