using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [DisplayName("Category Name")]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        Category Category { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal SellPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        public decimal Discount { get; set; }

        [Required]
        public int ImageUrl { get; set; }

        public int IsActive { get; set; } = 1;

    }
}
