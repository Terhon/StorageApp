using Microsoft.AspNetCore.Mvc;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.IngredientItem.Commands.DTOs;
using Storage.Application.Recipe.Commands;
using Storage.Application.Recipe.Commands.DTOs;
using Storage.Application.Recipe.Queries;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class RecipeController(
        IRecipeCommandService commandService, 
        IRecipeQueryService queryService,
        IIngredientItemCommandService ingredientItemCommandService) : Controller
    {
        // GET: Recipe
        public async Task<IActionResult> Index()
        {
            var recipes = await queryService.GetAllRecipes();
            return View(recipes.Map());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await queryService.GetRecipeById(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe.Map());
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
        public async Task<IActionResult> Create(Recipe recipe)
        {
            if (!ModelState.IsValid)
                return View(recipe);

            var id = await commandService.AddRecipe(new CreateRecipeCommand(recipe.Name, recipe.Description));
            
            foreach (var ingredient in recipe.Ingredients)
                await ingredientItemCommandService.AddIngredientItem(new CreateIngredientItemCommand(id, ingredient.ItemId ,ingredient.Amount));

            return RedirectToAction(nameof(Index));
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await queryService.GetRecipeById(id.Value);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe.Map());
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

            var existingRecipe = await queryService.GetRecipeById(id);
            if (existingRecipe == null)
                return NotFound();

            await commandService.UpdateRecipe(new UpdateRecipeCommand(existingRecipe.Id, updatedRecipe.Name, updatedRecipe.Description));
            
            var updatedIngredientIds = updatedRecipe.Ingredients.Select(i => i.Id).ToList();
            var ingredientsToRemove = existingRecipe.Ingredients
                .Where(i => !updatedIngredientIds.Contains(i.Id))
                .ToList();

            foreach (var ingredient in ingredientsToRemove)
            {
                //existingRecipe.Ingredients.Remove(ingredient);
                await ingredientItemCommandService.DeleteIngredientItem(new DeleteIngredientItemCommand(ingredient.Id));
            }

            // Add or update ingredients
            foreach (var ingredient in updatedRecipe.Ingredients)
            {
                var existingIngredient = existingRecipe.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                if (existingIngredient == null)
                {
                    //existingRecipe.Ingredients.Add(ingredient);
                    await ingredientItemCommandService.AddIngredientItem(new CreateIngredientItemCommand(existingRecipe.Id, ingredient.ItemId, ingredient.Amount));
                }
                else
                {
                    await ingredientItemCommandService.UpdateIngredientItem(
                        new UpdateIngredientItemCommand(existingIngredient.Id, existingRecipe.Id, ingredient.ItemId, ingredient.Amount));
                    //context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
                }
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

            var recipe = await queryService.GetRecipeById(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe.Map());
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await queryService.GetRecipeById(id);
            if (recipe != null)
            {
                await commandService.DeleteRecipe(new DeleteRecipeCommand(id));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}