namespace PathwayCoordinator.Interfaces;

public interface IPathwayStep
{
  public Task ExecuteAsync(dynamic details);
}
