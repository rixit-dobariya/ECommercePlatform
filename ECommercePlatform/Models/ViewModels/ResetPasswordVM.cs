using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Both passwords must be same")]
        public string ConfirmPassword { get; set; }
    }
}
