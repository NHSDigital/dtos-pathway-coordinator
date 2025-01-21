using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PathwaysController (ILogger<PathwaysController> logger) : ControllerBase
{
  [HttpGet]
  public ActionResult<List<Pathway>> GetPathways()
  {
    // Path to pathways.json in Configurations folder
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "pathways.json");

    if (!System.IO.File.Exists(filePath))
    {
      logger.LogInformation($"No pathways file found at {filePath}");
      return NotFound("Pathways file not found.");
    }

    // Read and deserialize pathways.json
    var json = System.IO.File.ReadAllText(filePath);
    var pathways = JsonSerializer.Deserialize<List<Pathway>>(json);

    return Ok(pathways);
  }
}
