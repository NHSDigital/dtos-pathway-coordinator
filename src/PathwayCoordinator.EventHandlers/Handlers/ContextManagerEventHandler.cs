using System.Text.Json;
using Azure.Messaging.EventGrid;
using ContextManager.API.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.EventHandler.Handlers;

public class ContextManagerEventHandler(ILogger<ContextManagerEventHandler> logger, IContextManagerService contextManagerApiClient)
{
    [Function(nameof(ContextManagerEventHandler))]

    public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
    {
        logger.LogInformation($"Received message : {eventGridEvent.Data}");
        var genericEvent = JsonSerializer.Deserialize<GenericEvent>(eventGridEvent.Data);
        logger.LogInformation(
            $"Received event on participant-events-topic/AuditSubscription  : {genericEvent?.EventName}");
        if (genericEvent != null) await contextManagerApiClient.AddEventAsync(genericEvent);
    }
}
