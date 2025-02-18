using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class ProductAttribute
    {
        [Key]
        public int AttributeID { get; set; }

        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Value { get; set; }
    }
}
