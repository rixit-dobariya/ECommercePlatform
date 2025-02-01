using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class OfferController : Controller
    {

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Offer";

            return View();
        }
    }
}
