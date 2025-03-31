using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        [ValidateNever]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Region is required.")]
        [StringLength(100, ErrorMessage = "Region name cannot exceed 100 characters.")]
        [Display(Name = "Region")]
        public string Region { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(100, ErrorMessage = "State name cannot exceed 100 characters.")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Pin Code is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be exactly 6 digits.")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Pin Code must be numeric and exactly 6 digits.")]
        [Display(Name = "Pin Code")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must contain only digits.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Is Deleted")]
        public int IsDeleted { get; set; } = 0;
    }
}
