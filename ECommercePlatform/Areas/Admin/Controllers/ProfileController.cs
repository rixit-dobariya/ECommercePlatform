using ECommercePlatform.Constants;
using ECommercePlatform.Helpers;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class ProfileController : Controller
    {
        private IWebHostEnvironment _env;
        private IUnitOfWork _unitOfWork;
        public ProfileController(IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Edit()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User user = await _unitOfWork.Users.Get(u => u.UserId == userId);
            AdminEditProfileVM adminEditProfileVM = new AdminEditProfileVM()
            {
                FullName = user.FullName,
                ProfilePictureUrl = user.ProfilePicture,
            };
            return View(new AdminProfileVM { AdminEditProfileVM= adminEditProfileVM });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(AdminEditProfileVM adminEditProfileVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", new AdminProfileVM { AdminEditProfileVM= adminEditProfileVM });
            }
            if (adminEditProfileVM.ProfilePicture != null)
            {
                if (!adminEditProfileVM.ProfilePicture.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("AdminEditProfileVM.ProfilePicture", "Profile Picture is not in the correct format. Please choose image.");
                    return View("Edit", new AdminProfileVM { AdminEditProfileVM = adminEditProfileVM });
                }
                else
                {
                    string wwwRootPath = _env.WebRootPath;
                    if (!string.IsNullOrEmpty(adminEditProfileVM.ProfilePictureUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, adminEditProfileVM.ProfilePictureUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(adminEditProfileVM.ProfilePicture.FileName);
                    string userPath = Path.Combine(_env.WebRootPath, @"Images\users");

                    using (var fileStream = new FileStream(Path.Combine(userPath, fileName), FileMode.Create))
                    {
                        await adminEditProfileVM.ProfilePicture.CopyToAsync(fileStream);
                    }
                    adminEditProfileVM.ProfilePictureUrl = @"\Images\users\" + fileName;
                }
            }
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User user = await _unitOfWork.Users.Get(u => u.UserId == userId);
            user.ProfilePicture = adminEditProfileVM.ProfilePictureUrl;
            user.FullName = adminEditProfileVM.FullName;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();
            TempData["success"] = "User information updated successfully!";

            return RedirectToAction("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePassword changePassword)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User user = await _unitOfWork.Users.Get(u => u.UserId == userId);
            AdminEditProfileVM adminEditProfileVM = new AdminEditProfileVM()
            {
                FullName = user.FullName,
                ProfilePictureUrl = user.ProfilePicture,
            };
            if (!ModelState.IsValid)
            {
                return View("Edit", new AdminProfileVM { AdminEditProfileVM = adminEditProfileVM,ChangePassword =changePassword });
            }
            if(!PasswordHelper.VerifyPassword(changePassword.CurrentPassword, user.Password))
            {
                ModelState.AddModelError("ChangePassword.CurrentPassword", "Incorrect old password");
                return View("Edit", new AdminProfileVM { AdminEditProfileVM = adminEditProfileVM, ChangePassword = changePassword });
            }
            user.Password = PasswordHelper.HashPassword(changePassword.Password);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();
            TempData["success"] = "Password changed successfully!";
            return View("Edit", new AdminProfileVM { AdminEditProfileVM = adminEditProfileVM});
        }
    }
}
