using AspNetCoreGeneratedDocument;
using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Helpers.EmailHelper;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class ResponseController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IEmailService _emailService;
        public ResponseController(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Response";
            return View();
        }
        public async Task<IActionResult> Reply(int id)
        {
            if (id <= 0)
            {
                return NotFound("No such response exists");
            }
            Response response = await _unitOfWork.Responses.Get(r=>r.ResponseId == id);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Reply(Response response)
        {
            if (response.ResponseId <= 0)
            {
                return NotFound("No such response exists");
            }
            if (string.IsNullOrEmpty(response.Reply))
            {
                TempData["error"] = "Reply was empty, so mail has not been sent";
                return RedirectToAction("Index");
            }
            Response responseToBeUpdated = await _unitOfWork.Responses.Get(r => r.ResponseId == response.ResponseId);
            responseToBeUpdated.Reply = response.Reply;

            await _emailService.SendEmail(responseToBeUpdated.Email, "Reply from Flone", response.Reply);

            _unitOfWork.Responses.Update(responseToBeUpdated);
            await _unitOfWork.Save();
            TempData["success"] = "Reply sent succcssfully!";
            return RedirectToAction("Index");
        }
        #region API ENDPOINTS
        public async Task<JsonResult> GetAll()
        {
            List<Response> responseList = await _unitOfWork.Responses.GetAll().ToListAsync();
            return Json(new { data = responseList });
        }
        public async Task<JsonResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No order found."
                });
            }
            Response response = await _unitOfWork.Responses.Get(r=> r.ResponseId == id);
            _unitOfWork.Responses.Remove(response);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Response Deleted Successfully!"
            });
        }
        #endregion
    }
}
