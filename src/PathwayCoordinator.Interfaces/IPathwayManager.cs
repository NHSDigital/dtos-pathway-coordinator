using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces;

public interface IPathwayManager
{
  Task ExecuteStepsAsync(Pathway pathway, GenericEvent triggerEvent);
}
