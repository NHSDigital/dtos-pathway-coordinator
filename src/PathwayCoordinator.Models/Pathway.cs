namespace PathwayCoordinator.Models;

public class Pathway
{
  public Pathway(string name, List<PathwayStep> steps)
  {
    Name = name;
    Steps = steps;
  }

  public Pathway()
  {
  }

  public string Name { get; set; }
  public List<PathwayStep> Steps { get; set; }
}
