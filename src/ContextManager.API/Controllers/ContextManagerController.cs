using ContextManager.API.Services;
using Microsoft.AspNetCore.Mvc;
using PathwayCoordinator.Models;

namespace ContextManager.API.Controllers;

[ApiController]
[Route("api/events")]
public class ContextManagerController(IContextManagerService contextManagerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] GenericEvent eventData)
    {
        try
        {
            await contextManagerService.AddEventAsync(eventData);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] string nhsNumber, [FromQuery] string pathway)
    {
        var events = await contextManagerService.GetEventsAsync(nhsNumber, pathway);
        return Ok(events);
    }
}