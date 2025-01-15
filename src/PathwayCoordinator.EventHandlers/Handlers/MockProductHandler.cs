using System.Text.Json;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.EventHandler.Handlers;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Clients;
using Shared.Clients.Interfaces;

namespace PathwayCoordinator.EventHandler.Handlers;

public class MockProductHandler(ILogger<MockProductHandler> logger, IPathwayApiClient client)
{
    [Function(nameof(MockProductHandler))]
    public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
    {
        logger.LogInformation($"Received message : {eventGridEvent.Data}");
        var genericEvent = JsonSerializer.Deserialize<GenericEvent>(eventGridEvent.Data);

        //I've received an event, my product will do some magic shizzle and afterwards will emit an event
        //for the pathway coordinator to deal with. We're going to put some janky waits in here to
        //see if we can tie this together a bit

        var publishEvent = new GenericEvent();
        publishEvent.Pathway = genericEvent.Pathway;
        publishEvent.NhsNumber = genericEvent.NhsNumber;
        if (genericEvent.TriggerEvent == "ParticipantEligible")
        {
            genericEvent.TriggerEvent = "ParticipantInvited";
            Thread.Sleep(1000);
        }

        await client.PublishEvent(JsonSerializer.Serialize(publishEvent));
    }
}
