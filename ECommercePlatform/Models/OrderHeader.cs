using ECommercePlatform.Constants;
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
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; } // Enum type

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  // UTC time by default

        [Required]
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }

        public int? BillingAddressId { get; set; }  // Nullable for optional billing address
        [ForeignKey("BillingAddressId")]
        public Address BillingAddress { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shipping Charge must be greater than zero.")]
        public decimal ShippingCharge { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than zero.")]
        public decimal Subtotal { get; set; }

        [Required]
        public PaymentMode PaymentMode { get; set; } // Enum type
        public bool IsDeleted { get; set; } = false;

        //Navigation property
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
