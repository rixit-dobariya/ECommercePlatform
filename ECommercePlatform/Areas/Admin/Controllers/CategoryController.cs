using ECommercePlatform.Constants;
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
        async Task<int> getChildCount(int? id)
        {
            return await _unitOfWork.Categories
                    .GetAll()
                    .CountAsync(c => c.ParentCategoryId == id);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CategoryVM categoryVM = new CategoryVM
            {
                Category = await _unitOfWork.Categories.Get(c => c.CategoryId == id),
            };

            //If a category has no child only, then it can have a parent category
            if (await getChildCount(id) == 0)
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

        #region API ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories
                                    .GetAll(includeProperties: "ParentCategory").ToListAsync();
            var categoriesList = await Task.WhenAll(categories.Select(async c => new
            {
                c.CategoryId,
                c.Name,
                ParentCategory = c.ParentCategory != null ? c.ParentCategory.Name : null,
                childCount = await getChildCount(c.CategoryId)
            }));
            return Json(new
            {
                data = categoriesList
            });
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
