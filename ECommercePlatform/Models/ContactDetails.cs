using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class ContactDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PhoneNumbers{ get; set; }
        [Required]
        public string Emails { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
