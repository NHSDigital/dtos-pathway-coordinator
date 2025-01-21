using PathwayCoordinator.Models;

namespace ContextManager.API.Services;

public interface IContextManagerService
{
    Task AddEventAsync(GenericEvent genericEvent);
    Task<List<GenericEvent>> GetEventsAsync(string nhsNumber, string pathway);

}