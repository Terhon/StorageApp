using Storage.Application.ItemType.Queries.DTOs;
using Storage.Application.Recipe.Queries.DTOs;

namespace Storage.Application.IngredientItem.Queries.DTOs;

public record IngredientItemDTO(int Id, RecipeDTO Recipe, ItemTypeDTO ItemType, double Amount);