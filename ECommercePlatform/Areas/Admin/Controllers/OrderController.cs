using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class OrderController : Controller
    {


        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Order";

            return View();
        }
    }
}
