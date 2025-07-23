using Storage.Application.ItemType.Queries.DTOs;

namespace Storage.Application.StorageItem.Queries.DTOs;

public class StorageItemDTO
{
    public int Id { get; set; }
    
    public ItemTypeDTO ItemType { get; set; }
    
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
}