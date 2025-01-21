using System.Text.Json;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Interfaces;

namespace PathwayCoordinator.EventHandler.Handlers;

public class ParticipantEventHandler(
  ILogger<ParticipantEventHandler> logger,
  IPathwayManager pathwayManager,
  IPathwayApiClient pathwayApiClient)
{
  [Function(nameof(ParticipantEventHandler))]
  public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
  {
    logger.LogInformation($"Received message : {eventGridEvent.Data}");
    var genericEvent = JsonSerializer.Deserialize<GenericEvent>(eventGridEvent.Data);
    //Based on generic event determines which is the trigger event for given pathway
    logger.LogInformation($"Received event on participant-events queue : {genericEvent.EventName}");
    var pathways = await pathwayApiClient.GetPathwaysAsync();
    var selectedPathway = pathways.FirstOrDefault(p => p.Name == genericEvent.Pathway);
    if (selectedPathway != null)
    {
      await pathwayManager.ExecuteStepsAsync(selectedPathway, genericEvent);
    }
  }
}
