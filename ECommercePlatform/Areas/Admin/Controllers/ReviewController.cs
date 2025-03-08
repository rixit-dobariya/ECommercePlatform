using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Reply(int id)
        {
            if (id <= 0)
            {
                return NotFound("No such response exists");
            }
            Review review = await _unitOfWork.Reviews.Get(r => r.ReviewId == id, "User,Product");
            return View(review);
        }
        [HttpPost]
        public async Task<IActionResult> Reply(Review review)
        {
            if (review.ReviewId <= 0)
            {
                return NotFound("No such review exists");
            }
            if (string.IsNullOrEmpty(review.ReplyText))
            {
                TempData["error"] = "Reply was empty, so it is not saved";
                return RedirectToAction("Index");
            }
            Review reviewToBeUpdated = await _unitOfWork.Reviews.Get(r => r.ReviewId == review.ReviewId);
            reviewToBeUpdated.ReplyText  = review.ReplyText;

            _unitOfWork.Reviews.Update(reviewToBeUpdated);
            await _unitOfWork.Save();
            TempData["success"] = "Reply sent succcssfully!";
            return RedirectToAction("Index");
        }
        #region API ENDPOINTS
        public async Task<JsonResult> GetAllForProduct(int id)
        {
            IQueryable<Review> reviews;
            reviews = _unitOfWork.Reviews.GetAll();
            var reviewsList= await _unitOfWork.Reviews.GetAll().Where(r => r.ProductId == id).Select(r => new
            {
                ReviewId=r.ReviewId,
                r.ReviewText,
                r.Rating,
                UserName = r.User.FullName,
                r.ReplyText
            }).ToListAsync();
            return Json(new
            {
                success = true,
                data = reviewsList
            });
        }
        public async Task<JsonResult> GetAll()
        {
            IQueryable<Review> reviews;
            reviews = _unitOfWork.Reviews.GetAll();
            var reviewsList = await _unitOfWork.Reviews.GetAll().Select(r => new
            {
                r.ReviewId,
                r.ReviewText,
                r.Rating,
                UserName = r.User.FullName,
                r.ReplyText,
                ProductImage = r.Product.ImageUrl,
                ProductName = r.Product.Name
            }).ToListAsync();
            return Json(new
            {
                success = true,
                data = reviewsList
            });
        }
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No record found!",
                });
            }
            Review review= await _unitOfWork.Reviews.Get(r => r.ReviewId == id);
            if (review == null)
            {
                return Json(new
                {
                    success = false,
                    message="No record found!",
                });
            }
            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Review Deleted successfully!",
            });
        }
        #endregion
    }
}
