using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models.ViewModels
{
    public class MyAccountVM
    {
        public ChangePassword ChangePassword { get; set; }
        public UpdateProfile UpdateProfile { get; set; }
        public IEnumerable<Address> Addresses { get; set; }  
    }
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "confirm Password must be at least 8 characters long.")]
        [Compare("Password", ErrorMessage = "Both passwords must be same.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateProfile
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? ProfilePicture { get; set; }

    }
}
