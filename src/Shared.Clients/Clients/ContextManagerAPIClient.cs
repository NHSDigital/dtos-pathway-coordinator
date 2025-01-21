using System.Text;
using System.Text.Json;
using ContextManager.API.Services;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Models;

namespace Shared.Clients.Clients;

public class ContextManagerApiClient(HttpClient httpClient, ILogger<AuditApiClient> logger) : IContextManagerService
{
    public async Task AddEventAsync(GenericEvent genericEvent)
    {
        var json = JsonSerializer.Serialize(genericEvent);
        logger.LogInformation($"Received content of {json}");

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/api/events", content);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation($"Event successfully sent to API: {genericEvent.NhsNumber}, {genericEvent.EventName}");
        }
        else
        {
            logger.LogError($"Failed to send event to API: {response}");
        }
    }

    public async Task<List<GenericEvent>> GetEventsAsync(string nhsNumber, string pathway)
    {
        var response = await httpClient.GetAsync($"/api/events?nhsnumber={nhsNumber}&pathway={pathway}");

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation($"Event successfully sent to API: {nhsNumber}, {pathway}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GenericEvent>>(content) ?? throw new InvalidOperationException();
        }
        else
        {
            logger.LogError($"Failed to send event to API: {response}");
        }
        throw new NotImplementedException();
    }
}