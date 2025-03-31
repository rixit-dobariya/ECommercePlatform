using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class OrderDetail
    {
        [Required]
        [DisplayName("Order ID")]
        public int OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }  // Nullable to prevent null reference issues

        [Required]
        [DisplayName("Product ID")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }  // Nullable to prevent potential errors

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Discount amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount must be greater than zero.")]
        [DisplayName("Discount Amount")]
        public decimal DiscountAmount { get; set; }
    }
}
