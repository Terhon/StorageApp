using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storage.Application.ItemType.Commands;
using Storage.Application.ItemType.Commands.DTOs;
using Storage.Application.ItemType.Queries;

namespace Storage.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemTypeController(IItemTypeCommandService commandService, IItemTypeQueryService queryService): ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var items = await queryService.GetAllItemTypes();
        return Ok(items);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody]CreateItemTypeCommand itemType)
    {
        await commandService.AddItemType(itemType);
        return CreatedAtAction(nameof(Post), itemType);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await commandService.DeleteItemType(new DeleteItemTypeCommand(id));
        return NoContent();
    }
}