using Storage.Application.IngredientItem.Queries.DTOs;

namespace Storage.Application.IngredientItem.Queries;

public interface IIngredientItemQueryService
{
    public Task<IngredientItemDTO> GetIngredientItemById(int id);
    public Task<IEnumerable<IngredientItemDTO>> GetAllIngredients();
}