using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class IngredientItemController : Controller
    {
        private readonly StorageWebContext _context;

        public IngredientItemController(StorageWebContext context)
        {
            _context = context;
        }

        // GET: IngredientItem
        public async Task<IActionResult> Index()
        {
            var storageWebContext = _context.IngredientItems.Include(i => i.Item).Include(i => i.Recipe);
            return View(await storageWebContext.ToListAsync());
        }

        // GET: IngredientItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientItem = await _context.IngredientItems
                .Include(i => i.Item)
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredientItem == null)
            {
                return NotFound();
            }

            return View(ingredientItem);
        }

        // GET: IngredientItem/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description");
            return View();
        }

        // POST: IngredientItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipeId,ItemId,Amount")] IngredientItem ingredientItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredientItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", ingredientItem.ItemId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // GET: IngredientItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientItem = await _context.IngredientItems.FindAsync(id);
            if (ingredientItem == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", ingredientItem.ItemId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // POST: IngredientItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeId,ItemId,Amount")] IngredientItem ingredientItem)
        {
            if (id != ingredientItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredientItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientItemExists(ingredientItem.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", ingredientItem.ItemId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // GET: IngredientItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientItem = await _context.IngredientItems
                .Include(i => i.Item)
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredientItem == null)
            {
                return NotFound();
            }

            return View(ingredientItem);
        }

        // POST: IngredientItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientItem = await _context.IngredientItems.FindAsync(id);
            if (ingredientItem != null)
            {
                _context.IngredientItems.Remove(ingredientItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientItemExists(int id)
        {
            return _context.IngredientItems.Any(e => e.Id == id);
        }
    }
}
