using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class OrderDetail
    {
        [Required]
        public int OrderHeaderId { get; set; }  // Renamed for consistency
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount must be greater than zero.")]
        public decimal DiscountAmount { get; set; }  // Clear naming
    }
}
