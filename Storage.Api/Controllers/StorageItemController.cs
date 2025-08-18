using Microsoft.AspNetCore.Mvc;
using Storage.Application.StorageItem.Commands;
using Storage.Application.StorageItem.Commands.DTOs;
using Storage.Application.StorageItem.Queries;

namespace Storage.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageItemController(IStorageItemCommandService commandService, IStorageItemQueryService queryService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await queryService.GetAllStorageItems();
        return Ok(items);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await queryService.GetStorageItemById(id);
        if(item == null)
            return NotFound();
        
        return Ok(item);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateStorageItemCommand storageItem)
    {
        await commandService.AddStorageItem(storageItem);
        return CreatedAtAction(nameof(Post), storageItem);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody]UpdateStorageItemCommand storageItem)
    {
        await commandService.UpdateStorageItem(storageItem);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await commandService.DeleteStorageItem(new DeleteStorageItemCommand(id));
        return NoContent();
    }
}