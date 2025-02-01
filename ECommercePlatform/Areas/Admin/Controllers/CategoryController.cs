using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    public class CategoryController : Controller
    {
        IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Category";
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category category = _unitOfWork.Categories.Get(c => c.CategoryId == id);
            if (category == null) //Insert page
            {
                return View();
            }
            else //Update Page
            {
                return View(category);
            }
        }

        #region API ENDPOINTS
        [HttpGet]
        public IActionResult GetAll(int draw, int start, int length, string search)
        {
            var categoriesList = _unitOfWork.Categories.GetAll(includeProperties: "ParentCategory").Select(c => new
            {
                c.CategoryId,
                c.Name,
                ParentCategory = c.ParentCategory != null ? new { c.ParentCategory.Name } : null
            });
            return Json(new {
                draw = draw,
                recordsTotal = categoriesList.Count(),
                recordsFiltered = categoriesList.Count(),
                data = categoriesList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        { 
            if(id==null || id == 0)
            {
                return Json(new {
                    success = false,
                    message ="No record found!", 
                });
            }
            Category category = _unitOfWork.Categories.Get(c=>c.CategoryId==id);
            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Save();
            return Json(new { 
                success = true,
                message = "Delete Successful!", 
            });
        }
        #endregion
    }
}
