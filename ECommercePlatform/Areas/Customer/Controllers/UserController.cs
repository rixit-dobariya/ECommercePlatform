﻿using ECommercePlatform.Constants;
using ECommercePlatform.Helpers;
using ECommercePlatform.Helpers.EmailHelper;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class UserController : Controller
    {
        IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment env,IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user, IFormFile? profilePicture) 
        {
            if (ModelState.IsValid)
            {
                User checkUser = _unitOfWork.Users.Get(u => u.Email.Equals(user.Email));
                if (checkUser!=null)
                {
                    if (checkUser.IsEmailVerified)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(user);
                    }
                    else
                    {
                        return SendVerificationEmail(checkUser);
                    }
                }
                if (profilePicture != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                    string usersPath = Path.Combine(_env.WebRootPath, @"Images/users");
                    using (var fileStream = new FileStream(Path.Combine(usersPath, fileName), FileMode.Create))
                    {
                        profilePicture.CopyTo(fileStream);
                    }
                    user.ProfilePicture = @"/Images/users/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("ProfilePicture", "You must add your profile picture");
                    return View(user);
                }
                user.Password = PasswordHelper.HashPassword(user.Password);
                _unitOfWork.Users.Add(user);
                _unitOfWork.Save();
                return SendVerificationEmail(user);
            }
            return View(user);
        }

        private IActionResult SendVerificationEmail(User user)
        {

            User newUser = _unitOfWork.Users.Get(u => u.Email == user.Email);
            int userId = newUser.UserId;

            UserOTP userOTP = _unitOfWork.UserOTPs.Get(uo=>uo.UserId==userId);
            //either user has expired otp or user do not have otp
            if (userOTP == null || userOTP.ExpirationTime < DateTime.UtcNow)
            {
                if (userOTP!=null && userOTP.ExpirationTime < DateTime.UtcNow)
                {
                    _unitOfWork.UserOTPs.Remove(userOTP);
                    //remove old otp
                }
                Random random = new Random();
                string otp = random.Next((int)Math.Pow(10, 5), (int)Math.Pow(10, 6)).ToString();

                userOTP = new()
                {
                    UserId = userId,
                    OTP = otp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(5)
                };
                // Send email synchronously
                string subject = "OTP For email verification!";
                string body = $"<h1>Hi {user.FullName},</h1><p>Your OTP is {userOTP.OTP}!</p>";

                _emailService.SendEmail(user.Email, subject, body);

                // Set session
                HttpContext.Session.SetString("TempUserId", userId.ToString());

                _unitOfWork.UserOTPs.Add(userOTP);
                _unitOfWork.Save();
                TempData["sucess"] = "Mail sent successfully!";
            }
            else
            {
                // Set session
                HttpContext.Session.SetString("TempUserId", userId.ToString());

                TempData["sucess"] = "Your old OTP is still valid!";
            }


            
            return RedirectToAction("OtpVerification");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            // Check if user exists and is not deleted
            User user = _unitOfWork.Users.Get(u => u.Email == loginVM.Email && !u.IsDeleted);

            if (user == null || !PasswordHelper.VerifyPassword(loginVM.Password, user.Password))
            {
                TempData["error"] = "Email or password is incorrect!";
                return View(loginVM);
            }

            if (!user.IsEmailVerified)
            {
                ModelState.AddModelError("Email", "Your email is not verified.");
                return View(loginVM);
                //send otp instead
            }

            // Set session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            TempData["success"] = "Login successful!";

            // Redirect based on role
            return user.Role.Equals(UserRole.Admin)
                ? RedirectToAction("Index", "Home", new { area = UserRole.Admin })
                : RedirectToAction("Index", "Home");
        }

        public IActionResult OtpVerification()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OTPVerification(UserOTP userOTP)
        {
            if (!ModelState.IsValid)
                return View(userOTP);
            string userId = HttpContext.Session.GetString("TempUserId");
            UserOTP userOTPFetched = _unitOfWork.UserOTPs.Get(uo => uo.UserId==Convert.ToInt32(userId));

            if (userOTPFetched == null)
            {
                ModelState.AddModelError("OTP", "Invalid OTP or User.");
                return View(userOTP);
            }

            if (userOTP.OTP != userOTPFetched.OTP)
            {
                ModelState.AddModelError("OTP", "Invalid OTP.");
                return View(userOTP);
            }

            if (userOTPFetched.ExpirationTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("OTP", "This OTP has expired.");
                return View(userOTP);
            }

            // Verify Email
            User user = _unitOfWork.Users.Get(u => u.UserId == userOTPFetched.UserId);
            user.IsEmailVerified = true;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            // Remove OTP after successful verification
            _unitOfWork.UserOTPs.Remove(userOTPFetched);
            _unitOfWork.Save();

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            TempData["success"] = "Your email is verified successfully!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        #region METHODS
        public bool ValidateOtp(string enteredOtp)
        {
            string storedOtp = HttpContext.Session.GetString("Otp");
            string storedTimestamp = HttpContext.Session.GetString("OtpTimestamp");

            if (string.IsNullOrEmpty(storedOtp) || string.IsNullOrEmpty(storedTimestamp))
            {
                return false;  // No OTP stored in session
            }

            // Check if OTP has expired (e.g., expired after 5 minutes)
            DateTime otpTimestamp = DateTime.Parse(storedTimestamp);
            if (DateTime.UtcNow - otpTimestamp > TimeSpan.FromMinutes(5))
            {
                return false;  // OTP expired
            }

            return storedOtp == enteredOtp;  // Compare entered OTP with stored OTP
        }
        #endregion 
    }
}



/*
 * 1. User tries to register 
 * 2. User gets OTP in inbox
 * 3. User might try to resend OTP
 * 4. User's registeration is sucessful
 * 5. User forgot his password -> send reset password link
 */