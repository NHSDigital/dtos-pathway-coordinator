using ContextManager.API.Data;
using ContextManager.API.Models;
using Microsoft.EntityFrameworkCore;
using PathwayCoordinator.Models;
using Pathway = ContextManager.API.Models.Pathway;

namespace ContextManager.API.Services;

public class ContextManagerService(ContextManagerDbContext dbContext) : IContextManagerService
{

    public async Task AddEventAsync(GenericEvent genericEvent)
    {
        // Check if the participant exists
        var participant = await dbContext.Participants
            .Include(p => p.Pathways)
            .ThenInclude(p => p.Events)
            .FirstOrDefaultAsync(p => p.NhsNumber == genericEvent.NhsNumber);

        if (participant == null)
        {
            // Create a new participant if it doesn't exist
            participant = new Participant
            {
                NhsNumber = genericEvent.NhsNumber,
                Pathways = new List<Pathway>()
            };

            dbContext.Participants.Add(participant);
        }

        // Check if the pathway exists for the participant
        var pathway = participant.Pathways.FirstOrDefault(p => p.Name == genericEvent.Pathway);
        if (pathway == null)
        {
            // Create a new pathway if it doesn't exist
            pathway = new Pathway
            {
                Name = genericEvent.Pathway,
                Events =  new List<GenericEvent>()
            };
            participant.Pathways.Add(pathway);
        }

        pathway.Events.Add(genericEvent);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GenericEvent>> GetEventsAsync(string nhsNumber, string pathway)
    {
        var query = dbContext.Participants.AsQueryable()
            .Where(e => e.NhsNumber == nhsNumber)
            .SelectMany(p => p.Pathways.Where(path => path.Name == pathway))
            .SelectMany(path => path.Events);
        return await query.ToListAsync();

    }
}