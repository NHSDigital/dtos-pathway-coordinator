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
        //see if we can tie this together a bit and load the events and determine the next one from the defined pathway

        var pathways = await client.GetPathwaysAsync();
        var selectedPathway = pathways.FirstOrDefault(p => p.Name == genericEvent.Pathway);
        var currentStepIndex = selectedPathway.Steps
            .FindIndex(s => s.TriggerEvent == genericEvent.EventName);

        var nextStep = currentStepIndex >= 0 && currentStepIndex < selectedPathway.Steps.Count - 1
            ? selectedPathway.Steps[currentStepIndex + 1]
            : null;
        if (nextStep != null)
        {
            var random = new Random();

            var delaySeconds = random.Next(1, 11); // Upper bound is exclusive

            Console.WriteLine($"Sleeping event {genericEvent.EventName} for {delaySeconds} seconds");

            await Task.Delay(delaySeconds * 1000);
            // Convert to milliseconds and sleep

            var publishEvent = new GenericEvent();
            publishEvent.Pathway = genericEvent.Pathway;
            publishEvent.NhsNumber = genericEvent.NhsNumber;
            publishEvent.EventName = nextStep?.TriggerEvent;
            publishEvent.Topic = "PathwayCoordinator";
            publishEvent.Payload = "{}";
            await client.PublishEvent(JsonSerializer.Serialize(publishEvent));
        }
        else
        {
            logger.LogInformation($"No more steps found for pathway {genericEvent.Pathway}");
        }

    }
}
