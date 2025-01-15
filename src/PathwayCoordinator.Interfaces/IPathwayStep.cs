using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces;

public interface IPathwayStep
{
  public Task ExecuteAsync(GenericEvent eventDetails);
}
