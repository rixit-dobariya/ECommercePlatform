using ECommercePlatform.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public RoleType Role { get; set; } = RoleType.User;

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Confirm Password must be between 8 and 100 characters.")]
        [Compare("Password", ErrorMessage = "Both passwords must match.")]
        [NotMapped]
        [ValidateNever] // Prevent unnecessary model binding validation
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string Phone { get; set; }

        public bool IsDeleted { get; set; } = false;

        [MaxLength(255, ErrorMessage = "Profile picture URL cannot exceed 255 characters.")]
        public string? ProfilePicture { get; set; }

        [Required]
        [Display(Name = "Email Verified")]
        public bool IsEmailVerified { get; set; } = false;
    }
}
