using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace Shared.Clients.Clients;

public class AuditApiClient (HttpClient httpClient, ILogger<AuditApiClient> logger): IAuditApiClient
{

  public async Task<bool> CreateAuditEvent(EventAudit eventAudit)
  {
    var json = JsonSerializer.Serialize(eventAudit);
    logger.LogInformation($"Received content of {json}");

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("/api/audit/events", content);

    if (response.IsSuccessStatusCode)
    {
      logger.LogInformation($"Event successfully sent to API: {eventAudit.NHSNumber}, {eventAudit.EventName}");
      return true;
    }
    else
    {
      logger.LogError($"Failed to send event to API: {response}");
      return false;
    }
  }

}
