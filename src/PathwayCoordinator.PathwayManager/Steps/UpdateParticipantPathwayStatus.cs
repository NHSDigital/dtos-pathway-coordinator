using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class UpdateParticipantPathwayStatus(ILogger<UpdateParticipantPathwayStatus> logger) : PathwayStepBase(logger)
  {
    public override async Task<Task> ExecuteAsync(GenericEvent details)
    {
      logger.LogInformation($"Would update the participant details to: {details}");
      return Task.CompletedTask;
    }
  }
}
