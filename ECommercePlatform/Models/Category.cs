using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Parent Category")]
        public int? ParentCategoryId { get; set; }

        [ValidateNever]
        [ForeignKey("ParentCategoryId")]
        public Category ParentCategory { get; set; }


    }
}
