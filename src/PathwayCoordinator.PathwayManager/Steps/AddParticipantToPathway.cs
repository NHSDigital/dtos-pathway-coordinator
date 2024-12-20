using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class AddParticipantToPathway (ILogger<AddParticipantToPathway> logger) : PathwayStepBase(logger)
  {
    public override Task ExecuteAsync(dynamic details)
    {
      Logger.LogInformation($"Adding participant to pathway: {details}");
      return Task.CompletedTask;
    }
  }
}
