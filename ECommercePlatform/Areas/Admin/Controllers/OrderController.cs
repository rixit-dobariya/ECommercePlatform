using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        #region API ENDPOINTS
        public JsonResult GetAll()
        {
            IEnumerable<OrderHeader> orders = _unitOfWork.OrderHeaders.GetAll("User").AsQueryable().Where(o => !o.IsDeleted);
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
        public JsonResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No order found."
                });
            }
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.Get(o => o.OrderId == id);
            orderHeader.IsDeleted = true;
            _unitOfWork.OrderHeaders.Update(orderHeader);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Order Deleted Successfully!"
            });
        }
        #endregion
    }
}
