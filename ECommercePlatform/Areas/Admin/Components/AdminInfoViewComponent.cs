using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Components
{
    public class AdminInfoViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminInfoViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            AdminInfoVM adminInfoVM = new();

            if (userId != null)
            {
                var userTemp = await _unitOfWork.Users.Get(u => u.UserId == userId);
                adminInfoVM.ProfilePictureUrl = userTemp?.ProfilePicture ?? "";
                adminInfoVM.Email = userTemp?.Email ?? "";
                adminInfoVM.Name = userTemp?.FullName ?? "Admin";
            }

            return View(adminInfoVM);
        }
    }
}
