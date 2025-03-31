using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class ContactDetails
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DisplayName("Phone Numbers")]
        public string PhoneNumbers { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DisplayName("Email Addresses")]
        public string Emails { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [DisplayName("Company Address")]
        public string Address { get; set; }
    }
}
