using ECommercePlatform.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }

        public int BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public Address BillingAddress { get; set; }

        [Required]
        public decimal ShippingCharge { get; set; }
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        public string PaymentMode { get; set; }
    }
}
