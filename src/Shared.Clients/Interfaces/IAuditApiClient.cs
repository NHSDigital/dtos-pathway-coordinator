using PathwayCoordinator.Models;

namespace PathwayCoordinator.Interfaces;

public interface IAuditApiClient
{
  Task<bool> CreateAuditEvent(EventAudit eventAudit);
}
