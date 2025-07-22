using Storage.Application.Queries.DTOs;

namespace Storage.Application.Queries;

public interface IItemTypeQueryService
{
    public Task<ItemTypeDTO> GetItemTypeById(int id);
    public Task<IEnumerable<ItemTypeDTO>> GetAllItemTypes();
    
}