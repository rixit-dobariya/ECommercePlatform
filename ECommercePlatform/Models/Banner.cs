using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }


        [Display(Name = "Title")]
        public string? Title { get; set; }  // Optional text shown on the banner



        [Display(Name = "Text")]
        public string? Text { get; set; }  // Optional text shown on the banner


        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
