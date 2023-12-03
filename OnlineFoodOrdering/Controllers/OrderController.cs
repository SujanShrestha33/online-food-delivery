// Controllers/OrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Data;
using OnlineFoodOrdering.Models.Entity;

public class OrderController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<ApplicationUsers> _userManager;

    public OrderController(AppDbContext dbContext, UserManager<ApplicationUsers> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ManageOrders()
    {
        try
        {
            var orders = await _dbContext.Orders.ToListAsync();
            return View(orders);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return View("Error", ex);
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ViewOrderDetails(int orderId)
    {
        try
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return View("Error", ex);
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
    {
        try
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ManageOrders");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return View("Error", ex);
        }
    }

    //[Authorize(Roles = "customer")]

    //public async Task<IActionResult> PlaceORder()
    //{
    //    try
    //}
}
