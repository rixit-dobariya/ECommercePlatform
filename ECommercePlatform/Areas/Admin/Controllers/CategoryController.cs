﻿using ECommercePlatform.Constants;
using ECommercePlatform.Filters;
using ECommercePlatform.Models;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Category";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CategoryVM categoryVM = new CategoryVM
            {
                Category = await _unitOfWork.Categories.Get(c => c.CategoryId == id),
            };

            //If a category has no child only, then it can have a parent category
            if (getChildCount(id) == 0)
            {
                categoryVM.CategoriesList = await _unitOfWork.Categories
                                                .GetAll("ParentCategory")
                                                .Where(c => c.CategoryId != id && c.ParentCategoryId == null)
                                                .Select(c => new SelectListItem
                                                {
                                                    Text = c.Name,
                                                    Value = c.CategoryId.ToString()
                                                }).ToListAsync();
            }
            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid) { 
                _unitOfWork.Categories.Update(categoryVM.Category);
                await _unitOfWork.Save();
                TempData["sucess"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryVM);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CategoryVM categoryVM = new CategoryVM
            {
                //parent categories
                CategoriesList = await _unitOfWork.Categories
                .GetAll("ParentCategory")
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()
                }).ToListAsync()
            };
            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(categoryVM.Category);
                await _unitOfWork.Save();
                TempData["sucess"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryVM);
            }
        }
        private int getChildCount(int? id)
        {
            return _unitOfWork.Categories
                    .GetAll()
                    .Count(c => c.ParentCategoryId == id);
        }
        #region API ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories
                                    .GetAll("ParentCategory").ToListAsync();
            var categoriesList = 
                categories.Select( c => new
                {
                    c.CategoryId,
                    c.Name,
                    ParentCategory = c.ParentCategory?.Name,
                    childCount = getChildCount(c.CategoryId),
                    productsCount=getProductsCount(c.CategoryId)
                });
            return Json(new
            {
                data = categoriesList
            });
        }

        private int getProductsCount(int categoryId)
        {
            int count = _unitOfWork.Products
                .GetAll()
                .Count();
            count += _unitOfWork.Products
                .GetAllDeletedProducts()
                .Count();

            return  count;
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id) 
        { 
            if(id==null || id == 0)
            {
                return Json(new {
                    success = false,
                    message ="No record found!", 
                });
            }
            Category category = await _unitOfWork.Categories.Get(c=>c.CategoryId==id);

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.Save();

            return Json(new { 
                success = true,
                message = "Delete Successful!", 
            });
        }
        #endregion
    }
}
