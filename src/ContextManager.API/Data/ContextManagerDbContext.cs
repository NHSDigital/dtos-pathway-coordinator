using ContextManager.API.Models;
using Microsoft.EntityFrameworkCore;
using PathwayCoordinator.Models;
using Pathway = ContextManager.API.Models.Pathway;

namespace ContextManager.API.Data;

public class ContextManagerDbContext(DbContextOptions<ContextManagerDbContext> options) : DbContext(options)
{
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Pathway> Pathways { get; set; }
    public DbSet<GenericEvent> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participant>()
            .HasKey(p => p.NhsNumber); // Explicitly set NHSNumber as the primary key

        modelBuilder.Entity<Pathway>()
            .HasKey(p => p.Name); // Explicitly set NHSNumber as the primary key

    }
}