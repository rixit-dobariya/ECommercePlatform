using ECommercePlatform.Constants;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
            return _unitOfWork.Categories
                    .GetAll()
                    .Count(c => c.ParentCategoryId == id);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            CategoryVM categoryVM = new CategoryVM
            {
                Category = _unitOfWork.Categories.Get(c => c.CategoryId == id),
            };

            //If a category has no child only, then it can have a parent category
            if (getChildCount(id) == 0)
            {
                categoryVM.CategoriesList = _unitOfWork.Categories
                                                .GetAll("ParentCategory")
                                                .Where(c => c.CategoryId != id && c.ParentCategoryId == null)
                                                .Select(c => new SelectListItem
                                                {
                                                    Text = c.Name,
                                                    Value = c.CategoryId.ToString()
                                                });
            }
            return View(categoryVM);
        }

        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid) { 
                _unitOfWork.Categories.Update(categoryVM.Category);
                _unitOfWork.Save();
                TempData["sucess"] = "Category updated successfully!";
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
            CategoryVM categoryVM = new CategoryVM
            {
                //parent categories
                CategoriesList = _unitOfWork.Categories
                .GetAll("ParentCategory")
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()
                })
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
                TempData["sucess"] = "Category created successfully!";
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
