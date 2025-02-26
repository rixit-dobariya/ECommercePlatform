using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class OfferController : Controller
    {
        IUnitOfWork _unitOfWork;
        public OfferController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Offer";

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Offers.Add(offer);
                await _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(offer);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Offer offer = await _unitOfWork.Offers.Get(o => o.OfferId == id);
            return View(offer);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Offers.Update(offer);
                await _unitOfWork.Save();
                TempData["success"] = "Offer Updated Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(offer);
            }
        }
        #region API ENDPOINTS
        public async Task<JsonResult> GetAll()
        {
            IEnumerable<Offer> offers = await _unitOfWork.Offers.GetAll().ToListAsync();
            return Json(new {
                success=true,
                data=offers
            });
        }
        public async Task<JsonResult> Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No offer found."
                });
            }
            Offer offer = await _unitOfWork.Offers.Get(o => o.OfferId == id);
            _unitOfWork.Offers.Remove(offer);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Offer Deleted Successfully!"
            });
        }
        #endregion
    }
}
