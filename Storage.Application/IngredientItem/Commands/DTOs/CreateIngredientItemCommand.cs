namespace Storage.Application.IngredientItem.Commands.DTOs;

public record CreateIngredientItemCommand(int RecipeId, int ItemTypeId, double Amount);