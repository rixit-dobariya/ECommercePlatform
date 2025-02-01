using ECommercePlatform.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public enum VerificationStatus
    { 
        Actived=1, Deactived=0
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Role { get;set; } = UserRole.Admin;
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
        public VerificationStatus Status { get; set; } = VerificationStatus.Actived;
        public string ProfilePicture { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; } = false;

    }

}
