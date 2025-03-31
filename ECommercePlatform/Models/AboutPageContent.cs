using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class AboutPageContent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 50, ErrorMessage = "Description must be between 50 and 2000 characters.")]
        [Display(Name = "Company Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vision is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Vision must be between 10 and 500 characters.")]
        [Display(Name = "Company Vision")]
        public string Vision { get; set; }

        [Required(ErrorMessage = "Mission is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Mission must be between 10 and 500 characters.")]
        [Display(Name = "Company Mission")]
        public string Mission { get; set; }

        [Required(ErrorMessage = "Goal is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Goal must be between 10 and 500 characters.")]
        [Display(Name = "Company Goal")]
        public string Goal { get; set; }

        [StringLength(2000, ErrorMessage = "Extra content must not exceed 2000 characters.")]
        [Display(Name = "Additional Information")]
        public string? ExtraContent { get; set; } // Optional field
    }
}
