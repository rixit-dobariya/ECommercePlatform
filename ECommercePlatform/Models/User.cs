using ECommercePlatform.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Role { get;set; } = UserRole.User;
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get;set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? ProfilePicture { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; } = false;

    }

}
