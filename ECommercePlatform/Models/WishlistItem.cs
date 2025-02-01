using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class WishlistItem
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }    

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
