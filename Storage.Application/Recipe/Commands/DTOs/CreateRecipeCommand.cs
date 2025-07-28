using Storage.Application.IngredientItem.Commands.DTOs;

namespace Storage.Application.Recipe.Commands.DTOs;

public record CreateRecipeCommand(string Name, string Description);