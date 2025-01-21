using Microsoft.Extensions.Logging;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces
{
  public abstract class PathwayStepBase(ILogger logger) : IPathwayStep
  {
    protected readonly ILogger Logger = logger;

    public abstract Task<Task> ExecuteAsync(GenericEvent details);
  }
}
