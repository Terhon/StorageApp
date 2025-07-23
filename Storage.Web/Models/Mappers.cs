using Storage.Application.ItemType.Queries.DTOs;
using Storage.Application.StorageItem.Queries.DTOs;

namespace StorageWeb.Models;

public static class Mappers
{
    public static ItemTypeDTO Map(this Item item)
    {
        return new ItemTypeDTO
        {
            Id = item.Id,
            Name = item.Name,
            Unit = item.Unit
        };
    }
    
    public static Item Map(this ItemTypeDTO item)
    {
        return new Item
        {
            Id = item.Id,
            Name = item.Name,
            Unit = item.Unit
        };
    }

    public static StorageItemDTO Map(this StorageItem item)
    {
        return new StorageItemDTO
        {
            Id = item.Id,
            ItemType = item.Item.Map(),
            AcquisitionDate = item.AcquisitionDate,
            Amount = item.Amount
        };
    }

    public static StorageItem Map(this StorageItemDTO item)
    {
        return new StorageItem
        {
            Id = item.Id,
            ItemId = item.ItemType.Id,
            Item = item.ItemType.Map(),
            AcquisitionDate = item.AcquisitionDate,
            Amount = item.Amount,
        };
    }
}