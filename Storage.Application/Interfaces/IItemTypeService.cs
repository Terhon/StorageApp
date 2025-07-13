using Storage.Application.DTOs;

namespace Storage.Application.Interfaces;

public interface IItemTypeService
{
    public Task CreateItemType(CreateItemTypeDTO item);
}