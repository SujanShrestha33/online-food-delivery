using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Entity;
using System;
using System.Threading.Tasks;

namespace OnlineFoodOrdering.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;

        public UserController(UserManager<ApplicationUsers> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get users with the "Customer" role
                var customerRole = "customer"; // Update with the actual role name
                var usersWithCustomerRole = await _userManager.GetUsersInRoleAsync(customerRole);

                ViewData["Users"] = usersWithCustomerRole;
                return View(usersWithCustomerRole);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex);
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUsers user)
        {
            try
            {
                if (id != user.Id)
                {
                    return NotFound(id);
                }

                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(id);
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex);
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex);
            }
        }
    }
}
