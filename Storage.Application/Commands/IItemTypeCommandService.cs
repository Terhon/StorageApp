namespace Storage.Application.Commands;

public interface IItemTypeCommandService
{
    public Task AddItemType(CreateItemTypeCommand cmd);
    public Task UpdateItemType(UpdateItemTypeCommand cmd);
    public Task DeleteItemType(DeleteItemTypeCommand cmd);
    
}