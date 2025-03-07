using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class AboutPageContent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vision is required.")]
        public string Vision { get; set; }

        [Required(ErrorMessage = "Mission is required.")]
        public string Mission { get; set; }

        [Required(ErrorMessage = "Goal is required.")]
        public string Goal { get; set; }

        public string? ExtraContent { get; set; } // Optional field
    }
}
