using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public short Rating { get; set; }
        public string ReviewText { get; set; }
        public string ReplyText { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
