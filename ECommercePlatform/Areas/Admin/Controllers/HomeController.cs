using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Home";

            DashboardVM dashboardVM = new DashboardVM
            {
                TotalUsers = await _unitOfWork.Users.GetAll().CountAsync(),
                TotalProducts = await _unitOfWork.Products.GetAll().CountAsync(),
                TotalSales = await _unitOfWork.OrderDetails.GetAll().SumAsync(o => o.Price * o.Quantity),
                TotalOrders = await _unitOfWork.OrderHeaders.GetAll().CountAsync()
            };

            return View(dashboardVM);
        }

    }
}
