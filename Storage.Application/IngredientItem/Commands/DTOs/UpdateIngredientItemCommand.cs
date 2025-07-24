namespace Storage.Application.IngredientItem.Commands.DTOs;

public record UpdateIngredientItemCommand(int RecipeId, int ItemTypeId, double Amount);