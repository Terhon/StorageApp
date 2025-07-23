using Microsoft.EntityFrameworkCore;
using Storage.Application.ItemType.Queries.DTOs;
using Storage.Application.StorageItem.Queries;
using Storage.Application.StorageItem.Queries.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services.StorageItem;

public class StorageItemQueryService(StorageDbContext context): IStorageItemQueryService
{
    public async Task<StorageItemDTO> GetStorageItemById(int id)
    {
        var storageItem = await context.StorageItems.AsNoTracking()
            .Include(storageItem => storageItem.ItemType)
            .FirstOrDefaultAsync(it => it.Id == id);
        if (storageItem == null)
            return null;
        
        return new StorageItemDTO
        {
            Id = storageItem.Id,
            ItemType = new ItemTypeDTO
            {
                Id = storageItem.ItemType.Id, 
                Name = storageItem.ItemType.Name, 
                Unit = storageItem.ItemType.Unit
            },
            AcquisitionDate = storageItem.AcquisitionDate,
            Amount =   storageItem.Amount,
        };
    }

    public async Task<IEnumerable<StorageItemDTO>> GetAllStorageItems()
    {
        return await context.StorageItems.AsNoTracking()
            .Include(storageItem => storageItem.ItemType)
            .Select(i => new StorageItemDTO
            {
                Id = i.Id,
                ItemType = new ItemTypeDTO{Id = i.ItemType.Id, Name = i.ItemType.Name, Unit = i.ItemType.Unit},
                AcquisitionDate = i.AcquisitionDate,
                Amount = i.Amount,
            })
            .ToListAsync();
    }
}