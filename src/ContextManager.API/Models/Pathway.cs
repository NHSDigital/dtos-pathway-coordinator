using PathwayCoordinator.Models;

namespace ContextManager.API.Models;

public class Pathway
{
    public string Name { get; set; }
    public List<GenericEvent> Events  { get; set; }
}