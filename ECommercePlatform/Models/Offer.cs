using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        [Required]
        public string OfferCode { get; set; }
        [Required]
        public string OfferDescription { get; set; }
        [Required]
        public decimal MinimumAmount {  get; set; }
        [Required]
        public decimal Discount {  get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
