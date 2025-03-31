using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name must not be empty!")]
        [DisplayName("Product Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Product short description must not be empty!")]
        [DisplayName("Short Description")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "Product long description must not be empty!")]
        [DisplayName("Long Description")]
        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Category must not be empty!")]
        [DisplayName("Category Name")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Product stock must not be empty!")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Sell Price must not be empty!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sell Price must be greater than zero.")]
        [DisplayName("Sell Price")]
        public decimal SellPrice { get; set; }

        [Required(ErrorMessage = "Cost Price must not be empty!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost Price must be greater than zero.")]
        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100%.")]
        public decimal Discount { get; set; }

        [DisplayName("Image URL")]
        public string? ImageUrl { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; } = true;  // Changed from `int` to `bool`
    }
}
