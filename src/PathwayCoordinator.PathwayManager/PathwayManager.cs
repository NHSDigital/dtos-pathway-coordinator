using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using PathwayCoordinator.PathwayManager.Utils;

namespace PathwayCoordinator.PathwayManager
{
  public class PathwayManager(ILogger<PathwayManager> logger, IServiceProvider serviceProvider) : IPathwayManager
  {
    public async Task ExecuteStepsAsync(Pathway pathway, GenericEvent genericEvent)
    {
      logger.LogInformation($"About to execute event {genericEvent.TriggerEvent} for pathway {pathway.Name}");
      var triggeredStep = pathway.Steps.FirstOrDefault(s => s.TriggerEvent == genericEvent.TriggerEvent);

      if (triggeredStep != null)
      {
        var step = Type.GetType($"PathwayCoordinator.PathwayManager.Steps.{triggeredStep.Type}");

        if (step != null && serviceProvider.GetService(step) is IPathwayStep pathwayStep)
        {
          var processor = new TemplateProcessor();
          var populatedTemplate = processor.PopulateTemplate(triggeredStep.MessageTemplate.ToString(), genericEvent.Payload);
          //Need to munge together the step template and some data somehow
          await pathwayStep.ExecuteAsync(populatedTemplate);
        }
        else
        {
          logger.LogError($"Could not find template for type {triggeredStep.Type}");
        }
      }
    }
  }
}
