using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Home";

            return View();
        }
    }
}
