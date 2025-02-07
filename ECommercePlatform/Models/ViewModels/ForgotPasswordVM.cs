using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage ="Email cannot be empty!")]
        public string Email { get; set; }
    }
}
