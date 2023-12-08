using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Data;
using OnlineFoodOrdering.Models.Entity;

namespace OnlineFoodOrdering.Controllers
{
    public class FoodsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FoodsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Foods
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Foods.Include(f => f.SubCategory);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .Include(f => f.SubCategory)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food, IFormFile photoFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (photoFile != null && photoFile.Length > 0)
                {
                    //food.Photo = "/images/" + Guid.NewGuid() + "_" + photoFile.FileName;
                    //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", food.Photo);
                    food.Photo = "/images/" + Guid.NewGuid() + "_" + photoFile.FileName;
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, food.Photo.TrimStart('/'));

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }
                }
                food.ModifiedAt = DateTime.Now;
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", food.SubCategoryId);
            return View(food);
        }



        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", food.SubCategoryId);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FoodId,FoodName,FoodDescription,SubCategoryId,Photo,Price")] Food food, IFormFile photoFile)
        {
            if (id != food.FoodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (photoFile != null)
                    {
                        //food.Photo = "/images/" + Guid.NewGuid() + "_" + imageFile.FileName;
                        //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", food.Photo);

                        food.Photo = "/images/" + Guid.NewGuid() + "_" + photoFile.FileName;
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, food.Photo.TrimStart('/'));

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }
                    }
                    food.ModifiedAt = DateTime.Now;
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.FoodId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", food.SubCategoryId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .Include(f => f.SubCategory)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Foods == null)
            {
                return Problem("Entity set 'AppDbContext.Foods'  is null.");
            }
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.FoodId == id);
        }
    }
}