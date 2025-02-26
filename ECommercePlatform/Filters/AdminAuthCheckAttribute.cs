using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using ECommercePlatform.Constants;

namespace ECommercePlatform.Filters
{
    public class AdminAuthCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetString("Role");
            if (userId == null)
            {
                var controller = (Controller)context.Controller;
                controller.TempData["error"] = "You must be logged in to access this page!";
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
            if (userRole == null || userRole != RoleType.Admin.ToString())
            {
                var controller = (Controller)context.Controller;
                controller.TempData["error"] = "You must be admin to access this page!";
                context.Result = new RedirectToActionResult("Login", "User", new { area = UserRole.Customer});
            }
            base.OnActionExecuting(context);
        }
    }

}
