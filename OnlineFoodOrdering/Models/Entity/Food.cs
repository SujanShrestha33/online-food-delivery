using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class Food
    {
        [Key]
        public int FoodId { get; set; }

        [Required]
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public SubCategories SubCategory { get; set; }

        [Required]
        public string Photo { get; set; }

        [Required]
        public double Price { get; set; }

        public string Availability { get; set; }

        public DateTime ModifiedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
