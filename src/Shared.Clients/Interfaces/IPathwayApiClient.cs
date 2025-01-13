using Microsoft.AspNetCore.Mvc;
using PathwayCoordinator.Models;

namespace Shared.Clients.Interfaces;

public interface IPathwayApiClient
{
  Task<List<Pathway>> GetPathwaysAsync();
  Task<HttpResponseMessage> PublishEvent([FromBody] string eventData);
}
