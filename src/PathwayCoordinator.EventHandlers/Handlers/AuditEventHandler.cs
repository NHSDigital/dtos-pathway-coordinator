using System.Text.Json;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.EventHandler.Handlers;

public class AuditEventHandler(ILogger<AuditEventHandler> logger, IAuditApiClient auditApiClient)
{
    [Function(nameof(AuditEventHandler))]

    public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
    {
        logger.LogInformation($"Received message : {eventGridEvent.Data}");
        var genericEvent = JsonSerializer.Deserialize<GenericEvent>(eventGridEvent.Data);
        //Based on generic event determines which is the trigger event for given pathway
        var eventAudit = new EventAudit(genericEvent)
        {
            Timestamp = eventGridEvent.EventTime,
            Duration = DateTime.UtcNow - eventGridEvent.EventTime,
            Version = eventGridEvent.DataVersion
        };
        //Based on generic event determines which is the trigger event for given pathway
        logger.LogInformation(
            $"Received event on participant-events-topic/AuditSubscription  : {eventAudit?.EventName}");
        if (eventAudit != null) await auditApiClient.CreateAuditEvent(eventAudit);
    }
}
