using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class AddParticipantToPathway (ILogger<AddParticipantToPathway> logger, IPathwayApiClient client) : PathwayStepBase(logger)
  {
    public override Task ExecuteAsync(GenericEvent details)
    {
      var output = JsonSerializer.Serialize(details);
      Logger.LogInformation($"Adding participant to pathway: {output}");
      //I'm going to directly put a message on the Participant Manager queue
      var participantManagerEvent = new GenericEvent();
      participantManagerEvent.TriggerEvent = "ParticipantManager";
      participantManagerEvent.Pathway = details.Pathway;
      participantManagerEvent.NhsNumber = details.NhsNumber;
      participantManagerEvent.Payload = details.Payload;
      client.PublishEvent(JsonSerializer.Serialize(participantManagerEvent));
      return Task.CompletedTask;
    }
  }
}
