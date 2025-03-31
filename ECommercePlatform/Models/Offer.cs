using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        [Required(ErrorMessage = "Offer code is required.")]
        [DisplayName("Offer Code")]
        public string OfferCode { get; set; }

        [Required(ErrorMessage = "Offer description is required.")]
        [DisplayName("Offer Description")]
        public string OfferDescription { get; set; }

        [Required(ErrorMessage = "Minimum amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum amount must be greater than zero.")]
        [DisplayName("Minimum Purchase Amount")]
        public decimal MinimumAmount { get; set; }

        [Required(ErrorMessage = "Discount value is required.")]
        [Range(0.01, 100, ErrorMessage = "Discount must be greater than zero and less than or equal to 100.")]
        [DisplayName("Discount Percentage")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Maximum discount amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Maximum discount amount must be greater than zero.")]
        [DisplayName("Maximum Discount Amount")]
        public decimal MaxDiscountAmount { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DisplayName("Offer Start Date")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DisplayName("Offer End Date")]
        public DateOnly EndDate { get; set; }
    }
}
