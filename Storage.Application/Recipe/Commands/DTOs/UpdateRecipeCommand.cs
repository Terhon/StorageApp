namespace Storage.Application.Recipe.Commands.DTOs;

public record UpdateRecipeCommand(int Id, string Name, string Description);