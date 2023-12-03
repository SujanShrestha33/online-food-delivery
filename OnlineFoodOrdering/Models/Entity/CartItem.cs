using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required(ErrorMessage = "Cart ID is required.")]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required(ErrorMessage = "Food ID is required.")]
        public int FoodId { get; set; }

        [ForeignKey("FoodId")]
        public Food Food { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
    }
}
