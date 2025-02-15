using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Components
{
    [Area(UserRole.Customer)]
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserInfoViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            UserInfoVM userInfoVM = new();

            if (!string.IsNullOrEmpty(userId))
            {
                int userIdInt = Convert.ToInt32(userId);

                userInfoVM.CartItems = _unitOfWork.CartItems.GetAll("Product")
                    .Where(ci => ci.UserId == userIdInt) ?? new List<CartItem>();

                userInfoVM.WishlistCount = _unitOfWork.WishlistItems.GetAll()
                    ?.Count(wi => wi.UserId == userIdInt) ?? 0;

                userInfoVM.Total = userInfoVM.CartItems
                    .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                userInfoVM.ProfilePictureUrl = _unitOfWork.Users.Get(u => u.UserId == userIdInt)?.ProfilePicture
                    ?? "";
            }
            userInfoVM.ShippingCharge = userInfoVM.Total >= 500 ? 0 : 50;
            userInfoVM.Total += userInfoVM.ShippingCharge;

            return View(userInfoVM);
        }
    }
}
