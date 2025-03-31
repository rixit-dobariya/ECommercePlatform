using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Filters
{
    public class AuthCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                var controller = (Controller)context.Controller;
                controller.TempData["error"] = "You must be logged in to access this page!";
                context.Result = new RedirectToActionResult("Login", "User", null);
            }

            base.OnActionExecuting(context);
        }
    }

}
