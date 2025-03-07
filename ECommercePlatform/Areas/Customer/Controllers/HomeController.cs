using System.Diagnostics;
using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Areas.Customer.Controllers
{
    [Area(UserRole.Customer)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork; 


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> About()
        {
            AboutPageContent aboutPageContent= await _unitOfWork.AboutPageContent.Get(a => a.Id == 1);
            return View(aboutPageContent);
        }
        public async Task<IActionResult> Contact()
        {
            ContactDetails contactDetails = await _unitOfWork.ContactDetails.Get(c => c.Id == 1);
            return View(new ContactVM { 
                PhoneNumbers = contactDetails.PhoneNumbers.Split(","),
                Emails = contactDetails.Emails.Split(","),
                Address = contactDetails.Address
            });
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Response response)
        {
            if (!ModelState.IsValid)
            {
                return View(new ContactVM { Response = response });
            }
            else
            {
                _unitOfWork.Responses.Add(response);
                await _unitOfWork.Save();
                TempData["success"] = "Your response has been stored successfully!";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
