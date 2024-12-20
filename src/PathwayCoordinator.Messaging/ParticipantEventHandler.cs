using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.Messaging;

public class ParticipantEventHandler(
  ILogger<ParticipantEventHandler> logger,
  IPathwayManager pathwayManager,
  IPathwayApiClient pathwayApiClient)
{
  [Function(nameof(ParticipantEventHandler))]
  public async Task Run(
    [ServiceBusTrigger("participant-events-topic", "PathwayInvocationSubscription", Connection = "ServiceBusConnection")] string message,
    FunctionContext context)
  {
    var genericEvent = JsonSerializer.Deserialize<GenericEvent>(message);
    //Based on generic event determines which is the trigger event for given pathway
    logger.LogInformation($"Received event on participant-events queue : {genericEvent.TriggerEvent}");
    var pathways = await pathwayApiClient.GetPathwaysAsync();
    var selectedPathway = pathways.FirstOrDefault(p => p.Name == genericEvent.Pathway);
    if (selectedPathway != null)
    {
      await pathwayManager.ExecuteStepsAsync(selectedPathway, genericEvent);
    }
  }
}
