using Storage.Application.Commands;

namespace Storage.Application.Interfaces;

public interface IItemTypeRepository
{
    Task AddItemType(CreateItemTypeCommand cmd);
}