using ECommercePlatform.Constants;
using ECommercePlatform.Models.ViewModels;
using ECommercePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using ECommercePlatform.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ECommercePlatform.Areas.Admin.Controllers
{
    [Area(UserRole.Admin)]
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
        public IActionResult Upsert(int? id = 0)
        {
            ProductVM productVM = new ProductVM
            {
                CategoriesList = _unitOfWork.Categories.GetAll()
                .Where(c => c.ParentCategoryId != null)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()
                })
            };
            if (id == null || id == 0)//Insert
            {
                return View(productVM);
            }
            else//Update
            {
                productVM.Product = _unitOfWork.Products.Get(p => p.ProductId == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? productImage)
        {
            if (ModelState.IsValid || productVM.Product?.ProductId==null)
            {
                if (productImage != null)
                {
                    if (productImage.ContentType.StartsWith("image/"))
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
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName);
                        string productPath = Path.Combine(_env.WebRootPath, @"Images\products");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            productImage.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\Images\products\" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("Product.ImageUrl", "Product Image is not in the correct format. Please choose image.");
                    }
                }
                if (productVM.Product.ProductId == 0)
                {
                    _unitOfWork.Products.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Products.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoriesList = _unitOfWork.Categories.GetAll()
                        .Where(c => c.ParentCategoryId != null)
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.CategoryId.ToString()
                        });
                return View(productVM);
            }
        }
        #region API ENDPOINTS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productsList = _unitOfWork.Products.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productsList });
           
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No record found!",
                });
            }
            Category category = _unitOfWork.Categories.Get(c => c.CategoryId == id);
            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Delete Successful!",
            });
        }
        #endregion
    }
}
