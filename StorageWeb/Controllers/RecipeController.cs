using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageWeb.Data;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class RecipeController(StorageWebContext context) : Controller
    {
        // GET: Recipe
        public async Task<IActionResult> Index()
        {
            return View(await context.Recipe.ToListAsync());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipe
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                context.Add(recipe);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipe);
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipe
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }
        
        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe updatedRecipe)
        {
            if (id != updatedRecipe.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(updatedRecipe);
            
            var existingRecipe = await context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRecipe == null)
                return NotFound();
            
            existingRecipe.Name = updatedRecipe.Name;
            existingRecipe.Description = updatedRecipe.Description;
            
            var updatedIngredientIds = updatedRecipe.Ingredients.Select(i => i.Id).ToList();
            var ingredientsToRemove = existingRecipe.Ingredients
                .Where(i => !updatedIngredientIds.Contains(i.Id))
                .ToList();
            
            foreach (var ingredient in ingredientsToRemove)
            {
                existingRecipe.Ingredients.Remove(ingredient);
                context.IngredientItems.Remove(ingredient);
            }
            
            // Add or update ingredients
            foreach (var ingredient in updatedRecipe.Ingredients)
            {
                var existingIngredient = existingRecipe.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                if (existingIngredient == null)
                {
                    existingRecipe.Ingredients.Add(ingredient);
                }
                else
                {
                    context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                context.Recipe.Remove(recipe);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return context.Recipe.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetIngredients(string search)
        {
            var ingredients = await context.Item
                .Where(i => i.Name.Contains(search))
                .Select(i => new
                {
                    id = i.Id,
                    name = i.Name,
                    unit = i.Unit
                })
                .ToListAsync();

            return Json(ingredients);
        }
    }
}