using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [AuthCheck]
    [Area(UserRole.Customer)]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Address address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }
            address.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _unitOfWork.Addresses.Add(address);
            await _unitOfWork.Save();
            TempData["success"] = "Address Added successfully!";
            return RedirectToAction("MyAccount", "User");
        }
        public async Task<IActionResult> Update(int addressId)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            Address address = await _unitOfWork.Addresses.Get(a => a.AddressId == addressId && a.UserId == userId);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Address address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }
            address.UserId = (int)HttpContext.Session.GetInt32("UserId");

            _unitOfWork.Addresses.Update(address);
            await _unitOfWork.Save();
            TempData["success"] = "Address Updated successfully!";
            return RedirectToAction("MyAccount","User");
        }
        public async Task<IActionResult> Delete(int addressId)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            Address address = await _unitOfWork.Addresses.Get(a => a.AddressId == addressId && a.UserId == userId);
            if (address == null) 
            {
                return NotFound();
            }
            _unitOfWork.Addresses.Remove(address);
            await _unitOfWork.Save();
            TempData["success"] = "Address deleted successfully";
            return RedirectToAction("MyAccount","User");
        }
    }
}
