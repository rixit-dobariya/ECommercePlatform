using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models.ViewModels
{
    public class AdminProfileVM
    {
        public ChangePassword ChangePassword { get; set; }
        
        public AdminEditProfileVM AdminEditProfileVM { get; set; }
    }
    public class AdminEditProfileVM
    {
        public string ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string FullName { get; set; }
    }
}
