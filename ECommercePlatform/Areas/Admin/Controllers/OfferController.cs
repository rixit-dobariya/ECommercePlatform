using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Offers.Add(offer);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(offer);
            }
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Offer offer = _unitOfWork.Offers.Get(o => o.OfferId == id);
            return View(offer);
        }
        [HttpPost]
        public IActionResult Edit(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Offers.Update(offer);
                _unitOfWork.Save();
                TempData["success"] = "Offer Updated Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(offer);
            }
        }
        #region API ENDPOINTS
        public JsonResult GetAll()
        {
            IEnumerable<Offer> offers = _unitOfWork.Offers.GetAll();
            return Json(new {
                success=true,
                data=offers
            });
        }
        public JsonResult Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No offer found."
                });
            }
            Offer offer = _unitOfWork.Offers.Get(o => o.OfferId == id);
            _unitOfWork.Offers.Remove(offer);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Offer Deleted Successfully!"
            });
        }
        #endregion
    }
}
