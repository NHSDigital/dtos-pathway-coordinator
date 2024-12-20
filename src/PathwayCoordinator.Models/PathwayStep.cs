namespace PathwayCoordinator.Models;

public class PathwayStep(string type, string triggerEvent, dynamic messageTemplate)
{
  public string Type { get; set; } = type; // Step type (e.g., SendEmail)
  public string TriggerEvent { get; set; } = triggerEvent; // Event that triggers this step
  public dynamic MessageTemplate { get; set; } = messageTemplate; // Additional messageTemplate for the step
}
