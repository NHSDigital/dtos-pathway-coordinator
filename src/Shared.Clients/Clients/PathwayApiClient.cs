using System.Net.Http.Json;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Interfaces;

namespace Shared.Clients.Clients;

public class PathwayApiClient(HttpClient httpClient): IPathwayApiClient
{
  // Fetch all pathways
  public async Task<List<Pathway>> GetPathwaysAsync()
  {
    var apiUrl = "/api/pathways"; // API base URL

    try
    {
      var pathways = await httpClient.GetFromJsonAsync<List<Pathway>>(apiUrl);
      return pathways ?? new List<Pathway>();
    }
    catch (Exception ex)
    {
      throw new ApplicationException($"Error fetching pathways from API: {ex.Message}", ex);
    }
  }

  public async Task<HttpResponseMessage> PublishEvent(string eventData)
  {
    var apiUrl = "/api/event"; // API base URL

    try
    {
      var response = await httpClient.PostAsJsonAsync(apiUrl, eventData);
      return response;
    }
    catch (Exception ex)
    {
      throw new ApplicationException($"Error publishing event: {ex.Message}", ex);
    }
  }
}
