using Storage.Application.IngredientItem.Commands.DTOs;

namespace Storage.Application.IngredientItem.Commands;

public interface IIngredientItemCommandService
{
    public Task AddIngredientItem(CreateIngredientItemCommand cmd);
    public Task UpdateIngredientItem(UpdateIngredientItemCommand cmd);
    public Task DeleteIngredientItem(DeleteIngredientItemCommand cmd);
}