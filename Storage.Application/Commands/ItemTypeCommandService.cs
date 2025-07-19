using Storage.Application.Interfaces;

namespace Storage.Application.Commands;

public class ItemTypeCommandService(IItemTypeRepository itemTypeRepository)
{
    public async Task AddItemType(CreateItemTypeCommand cmd)
    {
        await itemTypeRepository.AddItemType(cmd);
    }
}