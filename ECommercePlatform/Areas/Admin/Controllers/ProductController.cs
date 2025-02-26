using ECommercePlatform.Constants;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using ECommercePlatform.Filters;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
    [AdminAuthCheck]
    public class ProductController : Controller
    {
        IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Product";
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ProductVM productVM = new()
            {
                CategoriesList = await GetSelectListItems()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                if (productVM.productImage != null)
                {
                    if (productVM.productImage.ContentType.StartsWith("image/"))
                    {
                        string wwwRootPath = _env.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productVM.productImage.FileName);
                        string productPath = Path.Combine(_env.WebRootPath, @"Images\products");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await productVM.productImage.CopyToAsync(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\Images\products\" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("Product.ImageUrl", "Product Image is not in the correct format. Please choose image.");
                        productVM.CategoriesList = await GetSelectListItems();
                        return View(productVM);
                    }
                }
                else
                {
                    ModelState.AddModelError("Product.ImageUrl", "Product Image must not be empty.");
                    productVM.CategoriesList = await GetSelectListItems();
                    return View(productVM);
                }
                _unitOfWork.Products.Add(productVM.Product);
                await _unitOfWork.Save();
                TempData["sucess"] = "Product added successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoriesList = await GetSelectListItems();
                return View(productVM);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                if (productVM.productImage != null)
                {
                    if (productVM.productImage.ContentType.StartsWith("image/"))
                    {
                        string wwwRootPath = _env.WebRootPath;
                        if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productVM.productImage.FileName);
                        string productPath = Path.Combine(_env.WebRootPath, @"Images\products");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await productVM.productImage.CopyToAsync(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\Images\products\" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("Product.ImageUrl", "Product Image is not in the correct format. Please choose image.");
                        productVM.CategoriesList = await GetSelectListItems();
                        return View(productVM);
                    }
                }
                _unitOfWork.Products.Update(productVM.Product);
                await _unitOfWork.Save();
                TempData["sucess"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoriesList = await GetSelectListItems();
                return View(productVM);
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                ProductVM productVM = new()
                {
                    Product = await _unitOfWork.Products.Get(p => p.ProductId == id),
                    CategoriesList = await GetSelectListItems()
                };
                return View(productVM);
            }
        }
        #region METHODS
        async Task<IEnumerable<SelectListItem>> GetSelectListItems()
        {
            var categories = await _unitOfWork.Categories
                .GetAll("ParentCategory")
                .Where(c => c.ParentCategoryId != null && c.ParentCategory != null)
                .ToListAsync();

            var groupDictionary = new Dictionary<string, SelectListGroup>();

            IEnumerable<SelectListItem> categoriesList = categories
                    .GroupBy(c => c.ParentCategory!.Name)
                    .SelectMany(group => group.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryId.ToString(),
                        Group = groupDictionary.TryGetValue(group.Key, out var groupObj)
                            ? groupObj
                            : (groupDictionary[group.Key] = new SelectListGroup { Name = group.Key }) // Create if not exists
                    }))
                    .ToList();
            return categoriesList;
        }
        #endregion
        #region API ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Product> productsList = await _unitOfWork.Products.GetAll(includeProperties: "Category").ToListAsync();
            return Json(new { data = productsList });
           
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No record found!",
                });
            }
            Product product = await _unitOfWork.Products.Get(p => p.ProductId == id);
            product.IsActive = 0;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Delete Successful!",
            });
        }
        #endregion
    }
}
