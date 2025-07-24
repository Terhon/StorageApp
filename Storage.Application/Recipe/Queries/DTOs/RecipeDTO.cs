using Storage.Application.IngredientItem.Queries.DTOs;

namespace Storage.Application.Recipe.Queries.DTOs;

public record RecipeDTO(int Id, string Name, string Description, List<IngredientItemDTO> Ingredients);