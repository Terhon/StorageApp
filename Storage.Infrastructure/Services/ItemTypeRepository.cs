using Storage.Application.Commands;
using Storage.Application.Interfaces;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services;

public class ItemTypeRepository(StorageDbContext context) : IItemTypeRepository
{
    public async Task AddItemType(CreateItemTypeCommand cmd)
    {
        var entity = new ItemType { Name = cmd.Name, Unit = cmd.Unit };
        context.ItemTypes.Add(entity);
        await context.SaveChangesAsync();
    }
}