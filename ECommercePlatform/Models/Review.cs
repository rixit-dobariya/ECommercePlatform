using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public short Rating { get; set; }

        [Required(ErrorMessage = "Review text is required.")]
        [MaxLength(1000, ErrorMessage = "Review cannot exceed 1000 characters.")]
        public string ReviewText { get; set; }

        [MaxLength(1000, ErrorMessage = "Reply cannot exceed 1000 characters.")]
        public string? ReplyText { get; set; }

        // Navigation properties
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }
    }
}
