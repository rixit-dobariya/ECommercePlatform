using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ECommercePlatform.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [Required]
        public short Rating { get; set; }
        [Required]
        public string ReviewText { get; set; }
        public string? ReplyText { get; set; } 

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
