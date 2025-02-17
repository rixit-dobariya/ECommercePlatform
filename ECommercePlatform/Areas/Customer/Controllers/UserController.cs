using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Helpers;
using ECommercePlatform.Helpers.EmailHelper;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
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
            HttpContext.Session.SetInt32("UserId", user.UserId);
            TempData["success"] = "Login successful!";

            // Redirect based on role
            return (user.Role==RoleType.Admin)
                ? RedirectToAction("Index", "Home", new { area = UserRole.Admin })
                : RedirectToAction("Index", "Home");
        }

        public IActionResult OtpVerification()
        {
            int tempUserId = HttpContext.Session.GetInt32("TempUserId")??0;
            if (tempUserId == 0)
            {
                TempData["error"] = "You have not requested OTP yet.";
                return RedirectToAction("Index","User");
            }
            UserOTP userOTP = _unitOfWork.UserOTPs.Get(uo => uo.UserId == Convert.ToInt32(tempUserId));
            return View(new UserOTP { ExpirationTime=userOTP.ExpirationTime});
        }

        [HttpPost]
        public IActionResult OTPVerification(UserOTP userOTP)
        {
            if (!ModelState.IsValid)
                return View(userOTP);
            int? userId = HttpContext.Session.GetInt32("TempUserId");
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
            string whereToRedirect = HttpContext.Session.GetString("Page");
            if (whereToRedirect != null)
            {
                //go to reset password page
                HttpContext.Session.SetInt32("VerifiedOTP", 1);
                return RedirectToAction(whereToRedirect);
            }
            // Verify Email
            User user = _unitOfWork.Users.Get(u => u.UserId == userOTPFetched.UserId);
            user.IsEmailVerified = true;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            // Remove OTP after successful verification
            _unitOfWork.UserOTPs.Remove(userOTPFetched);
            _unitOfWork.Save();

            HttpContext.Session.SetInt32("UserId", user.UserId);
            TempData["success"] = "Your email is verified successfully!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResendOTP()
        {
            return SendVerificationEmail(
                new User{ Email = HttpContext.Session.GetString("TempEmail") }
                );
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Page", "ResetPassword");
                return SendVerificationEmail(new User{Email=forgotPasswordVM.Email});
            }
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            { 
                return View(resetPasswordVM);
            }
            if (HttpContext.Session.GetInt32("VerifiedOTP") == null) 
            { 
                TempData["error"] = "You cannot access this page without verifing your credentials!";
                return RedirectToAction("Index", "Home");
            }
            int? userId = HttpContext.Session.GetInt32("TempUserId");
            if (userId==null)
            {
                TempData["error"] = "You cannot access this page without verifing your credentials!";
                return RedirectToAction("Index", "Home");
            }
            User user = _unitOfWork.Users.Get(u => u.UserId == Convert.ToInt32(userId));
            user.Password = PasswordHelper.HashPassword(resetPasswordVM.Password);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            TempData["success"] = "Your password has been updated successfully!";
            return RedirectToAction("Index","Home");
        }

        [AuthCheck]
        public IActionResult MyAccount()
        {
            MyAccountVM myAccountVM = GetMyAccountVM();
            return View(myAccountVM);
        }
        [HttpPost]
        [AuthCheck]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            MyAccountVM myAccountVM = GetMyAccountVM();
            if (!ModelState.IsValid)
            {
                myAccountVM.ChangePassword = changePassword;
                return View("MyAccount", myAccountVM);
            }
            int? userId = HttpContext.Session.GetInt32("UserId"); 
            User user = _unitOfWork.Users.Get(u => u.UserId == userId);
            if (!PasswordHelper.VerifyPassword(changePassword.CurrentPassword, user.Password))
            {
                ModelState.AddModelError("ChangePassword.CurrentPassword","It is not your old password");
                myAccountVM.ChangePassword = changePassword;
                return View("MyAccount", myAccountVM);
            }
            user.Password = PasswordHelper.HashPassword(changePassword.Password);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            TempData["success"] = "Your password updated successfully!";
            return View("MyAccount", myAccountVM);
        }
        [HttpPost]
        [AuthCheck]
        public IActionResult UpdateProfile(UpdateProfile updateProfile, IFormFile? profilePicture)
        {
            MyAccountVM myAccountVM = new MyAccountVM();
            myAccountVM.UpdateProfile = updateProfile;
            myAccountVM.UpdateProfile.ProfilePicture = GetMyAccountVM().UpdateProfile.ProfilePicture;

            if (!ModelState.IsValid)
            {
                myAccountVM.Addresses = GetMyAccountVM().Addresses;
                return View("MyAccount", myAccountVM);
            }
            if (profilePicture != null)
            {
                if (!profilePicture.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("UpdateProfile.ProfilePicture", "Profile Picture is not in the correct format. Please choose image.");
                    myAccountVM.Addresses = GetMyAccountVM().Addresses;
                    return View("MyAccount", myAccountVM);
                }
                else
                {
                    string wwwRootPath = _env.WebRootPath;
                    if (!string.IsNullOrEmpty(updateProfile.ProfilePicture))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, updateProfile.ProfilePicture.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                    string userPath = Path.Combine(_env.WebRootPath, @"Images\users");

                    using (var fileStream = new FileStream(Path.Combine(userPath, fileName), FileMode.Create))
                    {
                        profilePicture.CopyTo(fileStream);
                    }
                    updateProfile.ProfilePicture = @"\Images\users\" + fileName;
                }
            }
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User user = _unitOfWork.Users.Get(u => u.UserId == userId);
            user.ProfilePicture = updateProfile.ProfilePicture;
            user.FullName = updateProfile.FullName;
            user.Email = updateProfile.Email;
            user.Phone = updateProfile.Phone;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            TempData["success"] = "User information updated successfully!";
            myAccountVM = GetMyAccountVM();
            return View("MyAccount", myAccountVM);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            TempData["success"] = "You have logged out successfully!";
            return RedirectToAction("Index","Home");
        }
        #region METHODS
        private MyAccountVM GetMyAccountVM()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            User user = _unitOfWork.Users.Get(u => u.UserId == userId);
            MyAccountVM myAccountVM = new MyAccountVM
            {
                UpdateProfile = new UpdateProfile
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    ProfilePicture = user.ProfilePicture
                },
                Addresses = _unitOfWork.Addresses.GetAll().Where(a => a.IsDeleted!=1 && a.UserId == userId)
            };
            return myAccountVM;
        }
        private IActionResult SendVerificationEmail(User user)
        {

            User newUser = _unitOfWork.Users.Get(u => u.Email == user.Email);
            int userId = newUser.UserId;

            UserOTP userOTP = _unitOfWork.UserOTPs.Get(uo => uo.UserId == userId);
            //either user has expired otp or user do not have otp
            if (userOTP == null || userOTP.ExpirationTime < DateTime.UtcNow)
            {
                if (userOTP != null && userOTP.ExpirationTime < DateTime.UtcNow)
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
                HttpContext.Session.SetInt32("TempUserId", userId);
                HttpContext.Session.SetString("TempEmail", user.Email);

                _unitOfWork.UserOTPs.Add(userOTP);
                _unitOfWork.Save();
                TempData["sucess"] = "Mail sent successfully!";
            }
            else
            {
                // Set session
                HttpContext.Session.SetInt32("TempUserId", userId);
                TempData["sucess"] = "Your old OTP is still valid!";
            }
            return RedirectToAction("OTPVerification");
        }

        #endregion 
    }
}
