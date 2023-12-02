using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUsers User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Canceled, InDelivery, Delivered

        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; } // This is the foreign key to the Order table
        [ForeignKey("OrderId")]
        public Order Order { get; set; } // Navigation property

        [Required]
        public int FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
