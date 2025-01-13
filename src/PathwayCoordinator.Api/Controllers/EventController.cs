using System.Text.Json;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(EventGridPublisherClient client) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PublishEvent([FromBody] string eventData)
    {
        var evt = JsonSerializer.Deserialize<GenericEvent>(eventData);

        EventGridEvent egEvent =
            new EventGridEvent(
                "/pathways/participants/12345",
                evt.TriggerEvent,
                "0.1",
                evt);
        // Send the event
        await client.SendEventAsync(egEvent);
        return Ok(new { Message = "Event published successfully" });
    }
}
