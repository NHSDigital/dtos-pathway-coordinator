using Microsoft.Extensions.Logging;

namespace PathwayCoordinator.Interfaces
{
  public abstract class PathwayStepBase(ILogger logger) : IPathwayStep
  {
    protected readonly ILogger Logger = logger;

    public abstract Task ExecuteAsync(dynamic details);
  }
}
