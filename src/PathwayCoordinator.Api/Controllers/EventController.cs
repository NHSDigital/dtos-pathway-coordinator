using System.Text.Json;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(ILogger<EventController> logger, EventGridClientFactory clientFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PublishEvent([FromBody] string eventData)
    {
        try
        {
            var evt = JsonSerializer.Deserialize<GenericEvent>(eventData);
            var client = clientFactory.CreateClient(evt.Topic);
            EventGridEvent egEvent =
                new EventGridEvent(
                    "/pathways/participants/12345",
                    evt.EventName,
                    "0.1",
                    evt);
            // Send the event
            await client.SendEventAsync(egEvent);
            return Ok(new { Message = "Event published successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(new { Message = ex.Message });
        }

    }
}


