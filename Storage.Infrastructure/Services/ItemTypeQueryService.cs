using Microsoft.EntityFrameworkCore;
using Storage.Application.Queries;
using Storage.Application.Queries.DTOs;
using Storage.Infrastructure.Data;

namespace Storage.Infrastructure.Services;

public class ItemTypeQueryService(StorageDbContext context) : IItemTypeQueryService
{
    public async Task<ItemTypeDTO> GetItemTypeById(int id)
    {
        var itemType = await context.ItemTypes.AsNoTracking().FirstOrDefaultAsync(it => it.Id == id);
        if (itemType == null)
            return null;
        
        return  new ItemTypeDTO
        {
            Id = itemType.Id,
            Name = itemType.Name,
            Unit = itemType.Unit
        };
    }

    public async Task<IEnumerable<ItemTypeDTO>> GetAllItemTypes()
    {
        return await context.ItemTypes.AsNoTracking()
            .Select(i => new ItemTypeDTO
            {
                Id = i.Id,
                Name = i.Name,
                Unit = i.Unit
            })
            .ToListAsync();
    }
}