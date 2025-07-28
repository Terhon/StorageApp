using Microsoft.EntityFrameworkCore;
using Storage.Application.Recipe.Queries;
using Storage.Application.Recipe.Queries.DTOs;
using Storage.Infrastructure.Data;
using Storage.Infrastructure.Services.IngredientItem;

namespace Storage.Infrastructure.Services.Recipe;

public class RecipeQueryService(StorageDbContext context) : IRecipeQueryService
{
    public async Task<RecipeDTO> GetRecipeById(int id)
    {
        var recipe = await context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(it => it.Id == id);
        if (recipe == null)
            return null;

        return new RecipeDTO(
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.Ingredients.Select(i => i.Map()).ToList());
    }

    public async Task<IEnumerable<RecipeDTO>> GetAllRecipes()
    {
        return await context.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Select(r => new RecipeDTO(
                r.Id,
                r.Name,
                r.Description,
                r.Ingredients.Select(i => i.Map()).ToList())
            )
            .ToListAsync();
    }
}