using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ECommercePlatform.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Product name must not be empty!")]
        public string? Name { get; set; }
        [Required(ErrorMessage ="Product short description must not be empty!")]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "Product long description must not be empty!")]
        public string? LongDescription { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage ="Category must not be empty!")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }

        [Required(ErrorMessage ="Product stock must not be empty!")]
        public int Stock { get; set; }

        [Required(ErrorMessage ="Sell Price must not be empty!")]
        public decimal SellPrice { get; set; }

        [Required(ErrorMessage ="Cost Price must not be empty!")]
        public decimal CostPrice { get; set; }

        public decimal Discount { get; set; }

        public string? ImageUrl { get; set; }

        public int IsActive {  get; set; } = 1;

    }
}
