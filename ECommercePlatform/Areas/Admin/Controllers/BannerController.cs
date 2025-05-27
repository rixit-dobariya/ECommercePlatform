using ECommercePlatform.Models;
using ECommercePlatform.Filters;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using ECommercePlatform.Constants;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class BannerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public BannerController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Banner";
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Banner banner, IFormFile? bannerImage)
        {
            if (ModelState.IsValid)
            {
                if (bannerImage != null && bannerImage.ContentType.StartsWith("image/"))
                {
                    string bannerPath = Path.Combine(_env.WebRootPath, @"Images\banners");
                    string fileName = Guid.NewGuid() + Path.GetExtension(bannerImage.FileName);
                    using (var fileStream = new FileStream(Path.Combine(bannerPath, fileName), FileMode.Create))
                    {
                        await bannerImage.CopyToAsync(fileStream);
                    }
                    banner.ImageUrl = @"\Images\banners\" + fileName;
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Please upload a valid image.");
                    return View(banner);
                }

                _unitOfWork.Banners.Add(banner);
                await _unitOfWork.Save();
                TempData["success"] = "Banner created successfully!";
                return RedirectToAction("Index");
            }
            return View(banner);
        }
       
        #region API ENDPOINTS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var banners = await _unitOfWork.Banners.GetAll().ToListAsync();
            return Json(new { data = banners });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _unitOfWork.Banners.Get(b => b.BannerId == id);
            if (banner == null)
            {
                return Json(new { success = false, message = "Banner not found!" });
            }
            _unitOfWork.Banners.Remove(banner);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Banner deleted!" });
        }
        #endregion
    }
}
