namespace PathwayCoordinator.Models;

public class GenericEvent
{
  public GenericEvent()
  {

  }
  public GenericEvent(string triggerEvent, string pathway, string payload)
  {
    TriggerEvent = triggerEvent;
    Pathway = pathway;
    Payload = payload;
  }

  public string TriggerEvent { get; set; } // The type of event (e.g., ParticipantInvited)
  public string Pathway { get; set; }  // The pathway name (e.g., Cancer Screening)

  public string NhsNumber { get; set; }
  public string Payload { get; set; } // Additional data specific to the event
}
