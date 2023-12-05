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
    public class SubCategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public SubCategoriesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SubCategories.Include(s => s.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .Include(s => s.Foods)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        public IActionResult Create(int? categoryId)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubCategoryId,SubCategoryName,SubCategoryDesc,CategoryId,ImageFile")] SubCategories subCategories)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (subCategories.ImageFile != null)
                {
                    //subCategories.SubCategoryImage = "/images/" + Guid.NewGuid() + "_" + subCategories.ImageFile.FileName;
                    //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", subCategories.SubCategoryImage);

                    subCategories.SubCategoryImage = "/images/" + Guid.NewGuid() + "_" + subCategories.ImageFile.FileName;
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,subCategories.SubCategoryImage.TrimStart('/'));


                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await subCategories.ImageFile.CopyToAsync(stream);
                    }
                }

                _context.Add(subCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Categories", new { id = subCategories.CategoryId });
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subCategories.CategoryId);
            return View(subCategories);
        }




        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subCategories = await _context.SubCategories.FindAsync(id);
            if (subCategories == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subCategories.CategoryId);
            return View(subCategories);
        }


        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubCategoryId,SubCategoryName,SubCategoryDesc,CategoryId,SubCategoryImage,ImageFile")] SubCategories subCategories)
        {
            if (id != subCategories.SubCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (subCategories.ImageFile != null)
                    {

                        subCategories.SubCategoryImage = "/images/" + Guid.NewGuid() + "_" + subCategories.ImageFile.FileName;
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, subCategories.SubCategoryImage.TrimStart('/'));


                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await subCategories.ImageFile.CopyToAsync(stream);
                        }
                    }

                    _context.Update(subCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoriesExists(subCategories.SubCategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subCategories.CategoryId);
            return View(subCategories);
        }
        // GET: SubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subCategories = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (subCategories == null)
            {
                return NotFound();
            }

            return View(subCategories);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubCategories == null)
            {
                return Problem("Entity set 'AppDbContext.SubCategories'  is null.");
            }
            var subCategories = await _context.SubCategories.FindAsync(id);
            if (subCategories != null)
            {
                _context.SubCategories.Remove(subCategories);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoriesExists(int id)
        {
          return _context.SubCategories.Any(e => e.SubCategoryId == id);
        }
    }
}
