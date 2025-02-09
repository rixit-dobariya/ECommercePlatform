using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class CartItem
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [ValidateNever]
        [Required]
        public int UserId { get; set; }



        [ValidateNever]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
