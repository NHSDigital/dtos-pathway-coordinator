using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Console = System.Console;
using Task = System.Threading.Tasks.Task;
namespace PathwayCoordinator.PathwayManager.Steps

{
  public class CreateParticipantEpisode (ILogger<CreateParticipantEpisode> logger) : PathwayStepBase(logger)
  {
    public override Task<Task> ExecuteAsync(GenericEvent details)
    {
      logger.LogInformation($"Creating an episode for participant: {details}");
      return Task.FromResult(Task.CompletedTask);
    }
  }
}
