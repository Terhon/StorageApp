using Microsoft.EntityFrameworkCore;
using Storage.Application.Commands;
using Storage.Domain.Entities;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services;

public class ItemTypeCommandService(StorageDbContext context) : IItemTypeCommandService
{
    public async Task AddItemType(CreateItemTypeCommand cmd)
    {
        var entity = new ItemType { Name = cmd.Name, Unit = cmd.Unit };
        context.ItemTypes.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateItemType(UpdateItemTypeCommand cmd)
    {
        var itemType = context.ItemTypes.First(x => x.Id == cmd.Id);
        if(itemType is null)
            throw new NullReferenceException($"ItemType with id {cmd.Id} not found");
        
        itemType.Name = cmd.Name;
        itemType.Unit = cmd.Unit;
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteItemType(DeleteItemTypeCommand cmd)
    {
        await context.ItemTypes.Where(it => it.Id == cmd.Id).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}