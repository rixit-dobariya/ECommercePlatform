using ECommercePlatform.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum PaymentMode
    {
        UPI,
        COD
    }

    public class OrderHeader
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [DisplayName("User ID")]
        public int UserId { get; set; }

        [Required]
        [DisplayName("Order Status")]
        public OrderStatus OrderStatus { get; set; } // Enum type

        [Required]
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  // UTC time by default

        [Required]
        [DisplayName("Shipping Address")]
        public int ShippingAddressId { get; set; }

        [DisplayName("Billing Address")]
        public int? BillingAddressId { get; set; }  // Nullable for optional billing address

        [Required(ErrorMessage = "Shipping charge is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shipping Charge must be greater than zero.")]
        [DisplayName("Shipping Charge")]
        public decimal ShippingCharge { get; set; }

        [Required(ErrorMessage = "Subtotal is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than zero.")]
        public decimal Subtotal { get; set; }

        [Required]
        [DisplayName("Payment Mode")]
        public PaymentMode PaymentMode { get; set; } // Enum type

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; } = false;

        // Navigation properties (nullable to prevent null reference issues)
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address? ShippingAddress { get; set; }

        [ForeignKey("BillingAddressId")]
        public Address? BillingAddress { get; set; }

        public IEnumerable<OrderDetail>? OrderDetails { get; set; }
    }
}
