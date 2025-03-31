using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [MaxLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
        public string Message { get; set; }

        [MaxLength(1000, ErrorMessage = "Reply cannot exceed 1000 characters.")]
        public string? Reply { get; set; }
    }
}
