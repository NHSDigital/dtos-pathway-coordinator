using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces;

public interface IPathwayApiClient
{
  Task<List<Pathway>> GetPathwaysAsync();
}
