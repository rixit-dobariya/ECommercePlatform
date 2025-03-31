using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        [ValidateNever]
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
