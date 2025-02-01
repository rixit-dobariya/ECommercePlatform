using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class ProductController : Controller
    {

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Product";

            return View();
        }
    }
}
