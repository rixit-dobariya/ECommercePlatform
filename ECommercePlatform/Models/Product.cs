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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [DisplayName("Category Name")]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal SellPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        public decimal Discount { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int IsActive {  get; set; } = 1;

    }
}
