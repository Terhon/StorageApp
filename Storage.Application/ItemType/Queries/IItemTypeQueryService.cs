using Storage.Application.ItemType.Queries.DTOs;

namespace Storage.Application.ItemType.Queries;

public interface IItemTypeQueryService
{
    public Task<ItemTypeDTO> GetItemTypeById(int id);
    public Task<IEnumerable<ItemTypeDTO>> GetAllItemTypes();
    
}