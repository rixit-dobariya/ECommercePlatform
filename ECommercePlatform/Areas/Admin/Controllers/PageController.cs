using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class PageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Contact()
        {
            ViewData["ActiveMenu"] = "Contact";
            ContactDetails contactDetails = await _unitOfWork.ContactDetails.Get(c => c.Id == 1);
            return View(contactDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactDetails contactDetails)
        {
            if (!ModelState.IsValid)
            {
                return View(contactDetails);
            }
            _unitOfWork.ContactDetails.Update(contactDetails);
            await _unitOfWork.Save();

            TempData["success"] = "Contact page details updated successfully!";
            return View(contactDetails);
        }
        public async Task<IActionResult> About()
        {
            ViewData["ActiveMenu"] = "About";
            AboutPageContent aboutPageContent = await _unitOfWork.AboutPageContent.Get(a => a.Id == 1);
            return View(aboutPageContent);
        }
        [HttpPost]
        public async Task<IActionResult> About(AboutPageContent aboutPageContent)
        {
            if (!ModelState.IsValid)
            {
                return View(aboutPageContent);
            }
            _unitOfWork.AboutPageContent.Update(aboutPageContent);
            await _unitOfWork.Save();
            return View(aboutPageContent);
        }
    }
}
