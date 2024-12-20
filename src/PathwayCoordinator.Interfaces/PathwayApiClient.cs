namespace PathwayCoordinator.Interfaces;

using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public class PathwayApiClient(HttpClient httpClient): IPathwayApiClient
{
  // Fetch all pathways
  public async Task<List<Pathway>> GetPathwaysAsync()
  {
    var apiUrl = "http://localhost:5100/api/pathways"; // API base URL

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
}
