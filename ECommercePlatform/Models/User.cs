using ECommercePlatform.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public RoleType Role { get;set; } = RoleType.User;
        [Required]
        public string FullName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get;set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "confirm Password must be at least 8 characters long.")]
        [Compare("Password",ErrorMessage ="Both passwords must be same.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ProfilePicture { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; } = false;

    }

}
