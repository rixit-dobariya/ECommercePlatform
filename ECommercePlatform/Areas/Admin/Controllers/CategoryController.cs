using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        int getChildCount(int? id)
        {
            return _unitOfWork.Categories.GetAll().Count(c => c.ParentCategoryId == id);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            IEnumerable<SelectListItem> categoriesList = _unitOfWork.Categories
                .GetAll("ParentCategory")
                .Where(c=>c.CategoryId!=id && c.ParentCategoryId==null)
                .Select(c =>   new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()
                });
            //find the childcategory count for condition
            int childCategoriesCount= getChildCount(id);

            Category category = _unitOfWork.Categories.Get(c => c.CategoryId == id);
            CategoryVM categoryVM = new CategoryVM
            {
                Category = category,
                CategoriesList = categoriesList
            };
            if (childCategoriesCount>=1)
            {
                categoryVM.CategoriesList = null; 
            }
            return View(categoryVM);
        }

        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid) { 
                _unitOfWork.Categories.Update(categoryVM.Category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryVM);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoriesList = _unitOfWork.Categories
                .GetAll("ParentCategory")
                .Where(c=>c.ParentCategoryId==null)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()
                });
            //Fetch parent categories for dropdown

            CategoryVM categoryVM = new CategoryVM
            {
                CategoriesList = categoriesList
            };
            return View(categoryVM);
        }

        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(categoryVM.Category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryVM);
            }
        }

        #region API ENDPOINTS
        [HttpGet]
        public IActionResult GetAll()
        {
            // Query with filtering
            var categoriesList = _unitOfWork.Categories
                .GetAll(includeProperties: "ParentCategory")
                .ToList().Select(c => new
                {
                    c.CategoryId,
                    c.Name,
                    c.ParentCategory,
                    childCount=getChildCount(c.CategoryId)
                });
            
            return Json(new
            {
                data = categoriesList
            });
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
