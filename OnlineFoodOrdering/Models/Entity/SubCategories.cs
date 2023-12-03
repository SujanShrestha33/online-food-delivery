using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class SubCategories
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "SubCategory Name is required.")]
        public string SubCategoryName { get; set; }

        [Required(ErrorMessage = "SubCategory Description is required.")]
        public string SubCategoryDesc { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }

        public ICollection<Food> Foods { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
