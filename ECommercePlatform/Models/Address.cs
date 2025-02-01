using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [MaxLength(6)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PinCode must be exactly 6 digits.")]
        public string PinCode { get; set; }

        [Required]
        public string Phone { get; set; }

    }
}
