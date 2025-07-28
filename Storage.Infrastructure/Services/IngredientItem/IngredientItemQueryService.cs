using Microsoft.EntityFrameworkCore;
using Storage.Application.IngredientItem.Queries;
using Storage.Application.IngredientItem.Queries.DTOs;
using Storage.Application.ItemType.Queries.DTOs;
using Storage.Application.Recipe.Queries.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services.IngredientItem;

public class IngredientItemQueryService(StorageDbContext context) : IIngredientItemQueryService
{
    public async Task<IngredientItemDTO> GetIngredientItemById(int id)
    {
        var ingredientItem = await context.IngredientItems.AsNoTracking()
            .Include(i => i.ItemType)
            .Include(i => i.Recipe)
            .FirstOrDefaultAsync(it => it.Id == id);
        
        return ingredientItem?.Map();
    }

    public async Task<IEnumerable<IngredientItemDTO>> GetAllIngredients()
    {
        return await context.IngredientItems.AsNoTracking()
            .Include(i => i.ItemType)
            .Include(i => i.Recipe)
            .Select(i => i.Map())
            .ToListAsync();
    }
}

public static class IngredientItemMapper
{
    public static IngredientItemDTO Map(this Domain.Entities.IngredientItem item)
    {
        return new IngredientItemDTO(item.Id,
            new RecipeDTO(
                item.Recipe.Id,
                item.Recipe.Name,
                item.Recipe.Description,
                null
            ),
            new ItemTypeDTO
            {
                Id = item.ItemType.Id,
                Name = item.ItemType.Name,
                Unit = item.ItemType.Unit,
            },
            item.Amount);
    }
}