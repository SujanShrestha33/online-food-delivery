﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Data;
using OnlineFoodOrdering.Models.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class OrderController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUsers> _userManager;

    public OrderController(AppDbContext context, UserManager<ApplicationUsers> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Display user's cart
    public async Task<IActionResult> ViewCart()
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Food).FirstOrDefault(c => c.UserId == user.Id);

        return View(cart);
    }


    // Add item to the cart
    [HttpPost]
    public async Task<IActionResult> AddToCart(int foodId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);

        // Retrieve or create a cart for the user
        var cart = _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Food)
            .FirstOrDefault(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart { UserId = user.Id, CartItems = new List<CartItem>() };
            _context.Carts.Add(cart);
        }

        var food = await _context.Foods.FindAsync(foodId);

        if (food != null)
        {
            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.FoodId == foodId);

            if (cartItem != null)
            {
                // Update quantity if the item is already in the cart
                cartItem.Quantity += quantity;
            }
            else
            {
                // Add a new item to the cart
                cart.CartItems.Add(new CartItem { Food = food, Quantity = quantity });
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ViewCart");
    }


    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string paymentMethod, string deliveryAddress)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Food)  // Include the associated Food entities
                .FirstOrDefault(c => c.UserId == user.Id);

            if (cart != null && cart.CartItems != null && cart.CartItems.Any())
            {
                var totalPrice = cart.CartItems.Sum(ci => ci.Food?.Price * ci.Quantity ?? 0);

                var order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    TotalPrice = totalPrice,
                    DeliveryAddress = deliveryAddress,
                    Status = "Pending",
                    PaymentMethod = paymentMethod,
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        FoodId = ci.FoodId,
                        Quantity = ci.Quantity,
                        Price = ci.Food?.Price ?? 0
                    }).ToList(),
                };

                _context.Orders.Add(order);

                // Clear the cart after placing the order
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();

                return RedirectToAction("ViewCart");
            }
        }
        return View("ViewCart");
    }
    public async Task<IActionResult> OrderHistory()
    {
        var user = await _userManager.GetUserAsync(User);
        var orders = _context.Orders
            .Include(o => o.User)  // Ensure the related user information is loaded
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }
    [Authorize]
    public async Task<IActionResult> ProvideFeedback(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order != null && order.Status == "Delivered")
        {
            return View(order);
        }
        else
        {
            // Handle invalid or non-delivered order
            return NotFound();
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitFeedback(int orderId, string feedback)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order != null && order.Status == "Delivered")
        {
            order.Feedback = feedback;
            await _context.SaveChangesAsync();

            // Redirect to order details or another appropriate page
            return RedirectToAction("OrderDetails", new { id = orderId });
        }
        else
        {
            // Handle invalid or non-delivered order
            return NotFound();
        }
    }





    // View user's orders
    public IActionResult ViewOrders()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var orders = _context.Orders.Where(o => o.UserId == user.Id).ToList();

        return View(orders);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ManageOrderStatus()
    {
        var orders = _context.Orders
            .Include(o => o.User)  // Ensure the related user information is loaded
            .ToList();

        return View(orders);
    }


    // Update order status (for admin)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ManageOrderStatus");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCartItemQuantity(int foodId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == user.Id);

        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.FoodId == foodId);

            if (cartItem != null)
            {
                // Update quantity
                cartItem.Quantity = quantity;

                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction("ViewCart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCartItem(int foodId)
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == user.Id);

        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.FoodId == foodId);

            if (cartItem != null)
            {
                // Remove the item from the cart
                cart.CartItems.Remove(cartItem);

                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction("ViewCart");
    }

    public IActionResult ChoosePaymentMethod()
    {
        return View();
    }

    // Display order details
    public IActionResult OrderDetails(int id)
    {
        // Retrieve the order from the database
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Food)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null)
        {
            return NotFound(); // Return a 404 Not Found if the order is not found
        }

        return View(order);
    }


}
