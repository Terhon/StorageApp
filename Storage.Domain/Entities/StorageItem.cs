namespace Storage.Domain.Entities;

public class StorageItem
{
    public int Id { get; set; }
    
    public ItemType ItemType { get; set; }
    
    public DateTime AcquisitionDate { get; set; }
    
    public int Amount { get; set; }
}