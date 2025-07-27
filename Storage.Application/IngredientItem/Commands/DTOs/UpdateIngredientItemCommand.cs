namespace Storage.Application.IngredientItem.Commands.DTOs;

public record UpdateIngredientItemCommand(int Id, int RecipeId, int ItemTypeId, double Amount);