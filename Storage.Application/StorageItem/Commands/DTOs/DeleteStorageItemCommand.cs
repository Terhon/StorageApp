namespace Storage.Application.StorageItem.Commands.DTOs;

public class DeleteStorageItemCommand(int id)
{
    public int Id { get; set; } = id;
}