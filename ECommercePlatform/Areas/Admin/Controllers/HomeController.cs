using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Home";

            return View();
        }
    }
}
