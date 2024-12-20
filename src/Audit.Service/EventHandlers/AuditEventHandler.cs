using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace Audit.Service.EventHandlers;

public class AuditEventHandler(ILogger<AuditEventHandler> logger, IAuditApiClient auditApiClient)
  {
    [Function(nameof(AuditEventHandler))]
    public async Task Run(
      [ServiceBusTrigger("participant-events-topic", "AuditSubscription", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message, FunctionContext context)
      {
        var genericEvent = JsonSerializer.Deserialize<GenericEvent>(message.Body);
        var eventAudit = new EventAudit(genericEvent);
        eventAudit.Timestamp = message.EnqueuedTime.UtcDateTime;
        eventAudit.Duration = DateTime.UtcNow - message.EnqueuedTime.UtcDateTime;
        //Based on generic event determines which is the trigger event for given pathway
        logger.LogInformation($"Received event on participant-events-topic/AuditSubscription  : {eventAudit?.EventName}");
        if (eventAudit != null) await auditApiClient.CreateAuditEvent(eventAudit);
      }
  }
