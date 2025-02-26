using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            UserInfoVM userInfoVM = new();

            if (userId != null)
            {
                userInfoVM.CartItems = await _unitOfWork.CartItems.GetAll("Product")
                    .Where(ci => ci.UserId == userId).ToListAsync() ?? new List<CartItem>();

                userInfoVM.WishlistCount = await _unitOfWork.WishlistItems
                    .GetAll()
                    .CountAsync(wi => wi.UserId == userId);

                userInfoVM.Total = userInfoVM.CartItems
                    .Select(ci => (ci.Product.SellPrice - ci.Product.SellPrice * ci.Product.Discount / 100) * ci.Quantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                var userTemp = await _unitOfWork.Users.Get(u => u.UserId == userId);
                userInfoVM.ProfilePictureUrl = userTemp?.ProfilePicture ?? "";
            }

            userInfoVM.ShippingCharge = userInfoVM.Total >= 500 ? 0 : 50;
            userInfoVM.Total += userInfoVM.ShippingCharge;

            return View(userInfoVM);
        }
    }
}
