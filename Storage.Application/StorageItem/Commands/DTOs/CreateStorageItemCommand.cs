namespace Storage.Application.StorageItem.Commands.DTOs;

public class CreateStorageItemCommand
{
    public int ItemTypeId { get; set; }
    
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
}