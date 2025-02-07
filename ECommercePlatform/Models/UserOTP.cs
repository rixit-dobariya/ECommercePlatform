using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class UserOTP
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        User user { get; set; }

        [Required(ErrorMessage = "OTP must not be empty.")]
        public string OTP { get; set; }

        [ValidateNever]
        public DateTime ExpirationTime { get; set; }
    }
}
