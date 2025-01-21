using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class MockProduct (ILogger<MockProduct> logger, IPathwayApiClient client) : PathwayStepBase(logger)
  {
    public override async Task<Task> ExecuteAsync(GenericEvent details)
    {
      try
      {
        var output = JsonSerializer.Serialize(details);
        logger.LogInformation($"About to invoke the mock product: {output}");
        //I'm going to directly put a message on the Participant Manager queue
        var participantManagerEvent = new GenericEvent();
        participantManagerEvent.EventName = details.EventName;
        participantManagerEvent.Pathway = details.Pathway;
        participantManagerEvent.NhsNumber = details.NhsNumber;
        participantManagerEvent.Payload = details.Payload;
        participantManagerEvent.Topic = "MockProduct";
        await client.PublishEvent(JsonSerializer.Serialize(participantManagerEvent));
        return Task.CompletedTask;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error publishing participant to pathway");
        return Task.FromException(ex);
      }

    }
  }
}
