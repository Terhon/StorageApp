using Microsoft.EntityFrameworkCore;
using Storage.Application.StorageItem.Commands;
using Storage.Application.StorageItem.Commands.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services.StorageItem;

public class StorageItemCommandService(StorageDbContext context) : IStorageItemCommandService
{
    public async Task AddStorageItem(CreateStorageItemCommand cmd)
    {
        var itemType = await context.ItemTypes.FirstAsync(x => x.Id == cmd.ItemTypeId);
        
        var entity = new Domain.Entities.StorageItem
        {
            Id = cmd.ItemTypeId,
            AcquisitionDate = cmd.AcquisitionDate,
            Amount = cmd.Amount,    
            ItemType = itemType,
        };
        await context.StorageItems.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStorageItem(UpdateStorageItemCommand cmd)
    {
        var storageItem = await context.StorageItems.FirstAsync(x => x.Id == cmd.Id);
        if (storageItem is null)
            throw new NullReferenceException($"StorageItem with id {cmd.Id} not found");

        storageItem.AcquisitionDate = cmd.AcquisitionDate;
        storageItem.Amount = cmd.Amount;
        
        var itemType = await context.ItemTypes.FirstAsync(x => x.Id == cmd.ItemTypeId);
        storageItem.ItemType = itemType;

        await context.SaveChangesAsync();
    }

    public async Task DeleteStorageItem(DeleteStorageItemCommand cmd)
    {
        await context.StorageItems.Where(it => it.Id == cmd.Id).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}