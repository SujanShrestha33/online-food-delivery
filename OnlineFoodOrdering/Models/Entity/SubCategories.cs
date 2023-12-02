using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class SubCategories
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required]
        public string SubCategoryName { get; set; }

        [Required]
        public string SubCategoryDesc { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }

        public ICollection<Food> Foods { get; set; }

        public DateTime modifiedAt { get; set; }
    }
}
