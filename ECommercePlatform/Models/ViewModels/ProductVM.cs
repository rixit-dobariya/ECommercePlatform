using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IFormFile productImage { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoriesList { get; set; }
        [ValidateNever]
        public List<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
    }
}
