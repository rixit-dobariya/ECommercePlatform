using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class ProductAttribute
    {
        [Key]
        public int AttributeID { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Attribute name is required.")]
        [Display(Name = "Attribute Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Attribute value is required.")]
        [Display(Name = "Attribute Value")]
        public string? Value { get; set; }
    }
}
