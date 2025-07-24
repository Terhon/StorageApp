using Storage.Application.Recipe.Queries.DTOs;

namespace Storage.Application.Recipe.Queries;

public interface IRecipeQueryService
{
    public Task<RecipeDTO> GetRecipeById(int id);
    public Task<IEnumerable<RecipeDTO>> GetAllRecipes();
}