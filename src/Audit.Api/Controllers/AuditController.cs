using Audit.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathwayCoordinator.Models;

namespace Audit.Api.Controllers;

[ApiController]
[Route("api/audit/")]
public class AuditController(AuditDbContext dbContext) : ControllerBase
{
  [HttpPost("events")]
  public async Task<IActionResult> CreateAuditEvent(EventAudit audit)
  {
    dbContext.EventAudits.Add(audit);
    await dbContext.SaveChangesAsync();
    return Ok();
  }

  [HttpGet("events")]
  public async Task<IActionResult> GetEvents([FromQuery] string? nhsNumber)
  {
    var query = dbContext.EventAudits.AsQueryable();

    if (!string.IsNullOrEmpty(nhsNumber))
    {
      query = query.Where(e => e.NHSNumber == nhsNumber).OrderBy(x => x.Timestamp);
    }

    return Ok(await query.OrderBy(x => x.Timestamp).ToListAsync());
  }
}
