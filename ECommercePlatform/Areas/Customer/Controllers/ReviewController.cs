using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [AuthCheck]
    [Area(UserRole.Customer)]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Review review)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(Request.Headers["Referer"]);
            }
            review.UserId = (int)HttpContext.Session.GetInt32("UserId");
            Review fetchedReview = await _unitOfWork.Reviews.Get(r => r.UserId == review.UserId && r.ProductId == review.ProductId);
            if (fetchedReview != null)
            {
                ModelState.AddModelError("Review.ReviewText","You have already added review");
                return Redirect(Request.Headers["Referer"]);
            }
            _unitOfWork.Reviews.Add(review);
            await _unitOfWork.Save();
            TempData["success"] = "Your review added successfully!";
            return Redirect(Request.Headers.Referer);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Review review)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(Request.Headers["Referer"]);
            }
            review.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.Save();
            TempData["success"] = "Your review updated successfully!";
            return Redirect(Request.Headers["Referer"]);
        }
        public async Task<IActionResult> Delete(int reviewId)
        {
            if (reviewId == 0)
            {
                return Redirect(Request.Headers["Referer"]);
            }
            Review review = await _unitOfWork.Reviews.Get(r => r.ReviewId == reviewId);
            if(review==null)
            {
                TempData["error"] = "Some problem occurred";
                return Redirect(Request.Headers["Referer"]);
            }
            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.Save();
            TempData["success"] = "Your review deleted successfully!";
            return Redirect(Request.Headers["Referer"]);
        }
    }
}
