using Storage.Application.Recipe.Commands.DTOs;

namespace Storage.Application.Recipe.Commands;

public interface IRecipeCommandService
{
    public Task<int> AddRecipe(CreateRecipeCommand cmd);
    public Task UpdateRecipe(UpdateRecipeCommand cmd);
    public Task DeleteRecipe(DeleteRecipeCommand cmd);
}