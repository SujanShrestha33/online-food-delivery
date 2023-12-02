using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineFoodOrdering.Models.Entity
{
    public class Categories
    {
        public Categories()
        {
            SubCategories = new List<SubCategories>(); // Initialize the collection in the constructor
        }

        [Key]
        public int CategoryId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }

        [DisplayName("Category Description")]
        public string Description { get; set; }

        public DateTime ModifiedDate { get; set; }

        public ICollection<SubCategories> SubCategories { get; set; }
    }
}
