using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "User";

            return View();
        }
    }
}
