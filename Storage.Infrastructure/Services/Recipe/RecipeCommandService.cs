using Microsoft.EntityFrameworkCore;
using Storage.Application.Recipe.Commands;
using Storage.Application.Recipe.Commands.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services.Recipe;

public class RecipeCommandService(StorageDbContext context): IRecipeCommandService
{
    public async Task AddRecipe(CreateRecipeCommand cmd)
    {
        var entity = new Domain.Entities.Recipe
        {
            Name = cmd.Name,
            Description = cmd.Description,
        };
        await context.Recipes.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRecipe(UpdateRecipeCommand cmd)
    {
        var recipe = await context.Recipes.FirstAsync(x => x.Id == cmd.Id);
        if (recipe is null)
            throw new NullReferenceException($"Recipe with id {cmd.Id} not found");

        recipe.Name = cmd.Name;
        recipe.Description = cmd.Description;

        await context.SaveChangesAsync();
    }

    public async Task DeleteRecipe(DeleteRecipeCommand cmd)
    {
        await context.Recipes.Where(it => it.Id == cmd.Id).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}