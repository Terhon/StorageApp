using Storage.Application.StorageItem.Queries.DTOs;

namespace Storage.Application.StorageItem.Queries;

public interface IStorageItemQueryService
{
    public Task<StorageItemDTO> GetStorageItemById(int id);
    public Task<IEnumerable<StorageItemDTO>> GetAllStorageItems();
}