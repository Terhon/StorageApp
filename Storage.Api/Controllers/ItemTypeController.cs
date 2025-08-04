using Microsoft.AspNetCore.Mvc;
using Storage.Application.ItemType.Commands;
using Storage.Application.ItemType.Queries;

namespace Storage.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemTypeController(IItemTypeCommandService commandService, IItemTypeQueryService queryService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await queryService.GetAllItemTypes();
        return Ok(items);
    }
}