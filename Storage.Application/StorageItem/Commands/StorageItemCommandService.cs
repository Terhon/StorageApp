using Storage.Application.StorageItem.Commands.DTOs;

namespace Storage.Application.StorageItem.Commands;

public interface IStorageItemCommandService
{
    public Task AddStorageItem(CreateStorageItemCommand cmd);
    public Task UpdateStorageItem(UpdateStorageItemCommand cmd);
    public Task DeleteStorageItem(DeleteStorageItemCommand cmd);
    
}