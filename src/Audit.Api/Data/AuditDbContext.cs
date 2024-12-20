using Microsoft.EntityFrameworkCore;
using PathwayCoordinator.Models;

namespace Audit.Api.Data;

public class AuditDbContext(DbContextOptions<AuditDbContext> options) : DbContext(options)
{
  public DbSet<EventAudit> EventAudits { get; set; }
}
