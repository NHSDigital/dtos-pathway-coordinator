using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class GetParticipantDetails(ILogger<GetParticipantDetails> logger) : PathwayStepBase(logger)
  {
    public override Task ExecuteAsync(dynamic details)
    {
      logger.LogInformation($"Going to retrieve participant details: {details}");
      return Task.CompletedTask;
    }
  }
}
