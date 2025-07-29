using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.IngredientItem.Commands.DTOs;
using Storage.Application.IngredientItem.Queries;
using Storage.Application.ItemType.Queries;
using Storage.Application.Recipe.Queries;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class IngredientItemController(
        IIngredientItemCommandService commandService,
        IIngredientItemQueryService queryService,
        IItemTypeQueryService itemTypeQueryService,
        IRecipeQueryService recipeQueryService) : Controller
    {
        // GET: IngredientItem
        public async Task<IActionResult> Index()
        {
            var ingredients = await queryService.GetAllIngredients();
            return View(ingredients.Map());
        }

        // GET: IngredientItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await queryService.GetIngredientItemById(id.Value);
            var ingredientItem = item?.Map();

            if (ingredientItem == null)
            {
                return NotFound();
            }

            return View(ingredientItem);
        }

        // GET: IngredientItem/Create
        public async Task<IActionResult> Create()
        {
            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name");

            var recipes = await recipeQueryService.GetAllRecipes();
            ViewData["RecipeId"] = new SelectList(recipes, "Id", "Description");
            return View();
        }

        // POST: IngredientItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngredientItem ingredientItem)
        {
            if (ModelState.IsValid)
            {
                await commandService.AddIngredientItem(new CreateIngredientItemCommand(ingredientItem.RecipeId,
                    ingredientItem.ItemId, ingredientItem.Amount));
                return RedirectToAction(nameof(Index));
            }

            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", ingredientItem.ItemId);

            var recipes = await recipeQueryService.GetAllRecipes();
            ViewData["RecipeId"] = new SelectList(recipes, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // GET: IngredientItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await queryService.GetIngredientItemById(id.Value);
            var ingredientItem = item.Map();
            if (ingredientItem == null)
            {
                return NotFound();
            }

            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", ingredientItem.ItemId);

            var recipes = await recipeQueryService.GetAllRecipes();
            ViewData["RecipeId"] = new SelectList(recipes, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // POST: IngredientItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IngredientItem ingredientItem)
        {
            if (id != ingredientItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await commandService.UpdateIngredientItem(
                        new UpdateIngredientItemCommand(
                            id,
                            ingredientItem.RecipeId,
                            ingredientItem.ItemId,
                            ingredientItem.Amount));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await IngredientItemExists(ingredientItem.Id))
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
            
            var itemTypes = await itemTypeQueryService.GetAllItemTypes();
            ViewData["ItemId"] = new SelectList(itemTypes, "Id", "Name", ingredientItem.ItemId);
            
            var recipes = await recipeQueryService.GetAllRecipes();
            ViewData["RecipeId"] = new SelectList(recipes, "Id", "Description", ingredientItem.RecipeId);
            return View(ingredientItem);
        }

        // GET: IngredientItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await queryService.GetIngredientItemById(id.Value);
            var ingredientItem = item.Map();
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
            var ingredientItem = await queryService.GetIngredientItemById(id);
            if (ingredientItem != null)
            {
                await commandService.DeleteIngredientItem(new DeleteIngredientItemCommand(id));
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IngredientItemExists(int id)
        {
            var items = await queryService.GetAllIngredients();
            return items.Any(e => e.Id == id);
        }
    }
}