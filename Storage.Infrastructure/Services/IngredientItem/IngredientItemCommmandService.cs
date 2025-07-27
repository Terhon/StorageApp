using Microsoft.EntityFrameworkCore;
using Storage.Application.IngredientItem.Commands;
using Storage.Application.IngredientItem.Commands.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services.IngredientItem;

public class IngredientItemCommandService(StorageDbContext context): IIngredientItemCommandService
{
    public async Task AddIngredientItem(CreateIngredientItemCommand cmd)
    {
        var itemType = await context.ItemTypes.FirstAsync(x => x.Id == cmd.ItemTypeId);
        var recipe = await context.Recipes.FirstAsync(x => x.Id == cmd.RecipeId);
        
        var entity = new Domain.Entities.IngredientItem
        {
            Recipe = recipe,
            ItemType = itemType,
            Amount = cmd.Amount,    
        };
        await context.IngredientItems.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateIngredientItem(UpdateIngredientItemCommand cmd)
    {
        var ingredientItem = await context.IngredientItems.FirstAsync(x => x.Id == cmd.Id);
        if (ingredientItem is null)
            throw new NullReferenceException($"IngredientItem with id {cmd.Id} not found");
        
        ingredientItem.Amount = cmd.Amount;
        
        var itemType = await context.ItemTypes.FirstAsync(x => x.Id == cmd.ItemTypeId);
        ingredientItem.ItemType = itemType;
        
        var recipe = await context.Recipes.FirstAsync(x => x.Id == cmd.RecipeId);
        ingredientItem.Recipe = recipe;

        await context.SaveChangesAsync();
    }

    public async Task DeleteIngredientItem(DeleteIngredientItemCommand cmd)
    {
        await context.IngredientItems.Where(it => it.Id == cmd.Id).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}