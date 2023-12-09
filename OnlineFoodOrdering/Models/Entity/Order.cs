using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Models.Entity
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUsers User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Price is required.")]
        public double TotalPrice { get; set; }

        [Required(ErrorMessage = "Delivery Address is required.")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } // Pending, Canceled, InDelivery, Delivered

        public string PaymentMethod { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string Feedback {  get; set; }

        public string StripePaymentIntentId { get; set; }
    }
}
