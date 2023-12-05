using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class Food
    {
        [Key]
        public int FoodId { get; set; }

        [Required(ErrorMessage = "Food Name is required.")]
        public string FoodName { get; set; }

        public string FoodDescription { get; set; }

        [Required(ErrorMessage = "SubCategory is required.")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public SubCategories SubCategory { get; set; }

     
        public string Photo { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public double Price { get; set; }

        public string Availability { get; set; }

        public DateTime ModifiedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
