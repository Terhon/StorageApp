namespace Storage.Application.StorageItem.Commands.DTOs;

public class UpdateStorageItemCommand
{
    public int Id { get; set; }
    
    public int ItemTypeId { get; set; }
    
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
}