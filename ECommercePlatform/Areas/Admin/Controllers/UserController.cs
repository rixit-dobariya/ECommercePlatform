﻿using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Helpers;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class UserController : Controller
    {
        IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "User";

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (userVM.profilePicture != null)
                {
                    if (userVM.profilePicture.ContentType.StartsWith("image/"))
                    {
                        string wwwRootPath = _env.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(userVM.profilePicture.FileName);
                        string productPath = Path.Combine(_env.WebRootPath, @"Images\users");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await userVM.profilePicture.CopyToAsync(fileStream);
                        }
                        userVM.User.ProfilePicture = @"\Images\users\" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("User.ProfilePicture", "User profile picture is not in the correct format. Please choose image.");
                        return View(userVM);
                    }
                }
                else
                {
                    ModelState.AddModelError("User.ProfilePicture", "User profile picture must not be empty.");
                    return View(userVM);
                }
                userVM.User.Password = PasswordHelper.HashPassword(userVM.User.Password);
                userVM.User.IsEmailVerified = true;
                _unitOfWork.Users.Add(userVM.User);
                await _unitOfWork.Save();
                TempData["sucess"] = "User added successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(userVM);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            UserVM userVM = new()
            {
                User = await _unitOfWork.Users.Get(u => u.UserId == id)
            };
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (userVM.profilePicture != null)
                {
                    if (userVM.profilePicture.ContentType.StartsWith("image/"))
                    {
                        string wwwRootPath = _env.WebRootPath;
                        if (!string.IsNullOrEmpty(userVM.User.ProfilePicture))
                        {
                            string oldImagePath = Path.Combine(wwwRootPath, userVM.User.ProfilePicture.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(userVM.profilePicture.FileName);
                        string productPath = Path.Combine(_env.WebRootPath, @"Images\users");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await userVM.profilePicture.CopyToAsync(fileStream);
                        }
                        userVM.User.ProfilePicture = @"\Images\users\" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("User.ProfilePicture", "Profile Picture is not in the correct format. Please choose image.");
                        return View(userVM);
                    }
                }
                _unitOfWork.Users.Update(userVM.User);
                await _unitOfWork.Save();
                TempData["sucess"] = "User updated successfully!";
                return RedirectToAction("Index");
            }
            return View(userVM);
        }

        #region API ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<User> usersList = await _unitOfWork.Users.GetAll().Where(u => u.Role==RoleType.User).ToListAsync();
            return Json(new { data = usersList });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No record found!",
                });
            }
            User user = await _unitOfWork.Users.Get(u => u.UserId == id);
            user.IsDeleted = true;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "User Deleted successfully!",
            });
        }
        #endregion
    }
}
