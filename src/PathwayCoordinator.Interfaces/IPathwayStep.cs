using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces;

public interface IPathwayStep
{
  public Task<Task> ExecuteAsync(GenericEvent eventDetails);
}
