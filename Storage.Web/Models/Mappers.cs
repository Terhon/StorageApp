using Storage.Application.IngredientItem.Queries.DTOs;
using Storage.Application.ItemType.Queries.DTOs;
using Storage.Application.Recipe.Queries.DTOs;
using Storage.Application.StorageItem.Queries.DTOs;

namespace StorageWeb.Models;

public static class Mappers
{
    public static ItemTypeDTO Map(this Item item)
    {
        return new ItemTypeDTO
        {
            Id = item.Id,
            Name = item.Name,
            Unit = item.Unit
        };
    }
    
    public static Item Map(this ItemTypeDTO item)
    {
        return new Item
        {
            Id = item.Id,
            Name = item.Name,
            Unit = item.Unit
        };
    }

    public static StorageItemDTO Map(this StorageItem item)
    {
        return new StorageItemDTO
        {
            Id = item.Id,
            ItemType = item.Item.Map(),
            AcquisitionDate = item.AcquisitionDate,
            Amount = item.Amount
        };
    }

    public static StorageItem Map(this StorageItemDTO item)
    {
        return new StorageItem
        {
            Id = item.Id,
            ItemId = item.ItemType.Id,
            Item = item.ItemType.Map(),
            AcquisitionDate = item.AcquisitionDate,
            Amount = item.Amount,
        };
    }

    public static IEnumerable<IngredientItem> Map(this IEnumerable<IngredientItemDTO> items)
    {
        return items.Select(i => i.Map());
    }
    
    public static IngredientItem Map(this IngredientItemDTO item)
    {
        return new IngredientItem
        {
            Id = item.Id,
            Amount = item.Amount,
            ItemId = item.ItemType.Id,
            Item = item.ItemType.Map(),
            RecipeId = item.Recipe.Id
            //Recipe = item,
        };
    }

    public static IEnumerable<Recipe> Map(this IEnumerable<RecipeDTO> items)
    {
        return items.Select(i => i.Map());
    }
    
    public static Recipe Map(this RecipeDTO item)
    {
        return new Recipe
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Ingredients = item.Ingredients.Map().ToList()
        };
    }
}