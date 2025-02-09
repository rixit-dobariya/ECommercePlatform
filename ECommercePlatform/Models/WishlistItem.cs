using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class WishlistItem
    {
        public int ProductId { get; set; }
        [ValidateNever]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }    

        public int UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
