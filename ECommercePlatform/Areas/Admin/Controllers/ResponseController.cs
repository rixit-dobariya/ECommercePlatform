using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class ResponseController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public ResponseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Response";
            return View();
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
