using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommercePlatform.Models.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoriesList { get; set; }
    }
}
