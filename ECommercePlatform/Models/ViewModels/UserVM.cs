using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ECommercePlatform.Models.ViewModels
{
    public class UserVM
    {
        [ValidateNever]
        public User User { get; set; }
        [ValidateNever]
        public IFormFile? profilePicture { get; set; }
    }
}
