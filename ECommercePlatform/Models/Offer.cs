using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        [Required]
        public string OfferCode { get; set; }  // Nullable removed since it's required

        [Required]
        public string OfferDescription { get; set; }  // Nullable removed since it's required

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum Amount must be greater than zero.")]
        public decimal MinimumAmount { get; set; }

        [Required]
        [Range(0.01, 100, ErrorMessage = "Discount must be greater than zero and less than or equal to 100.")]
        public decimal Discount { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }
    }
}
