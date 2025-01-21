namespace PathwayCoordinator.Models;

public class GenericEvent
{
  public GenericEvent()
  {

  }
  public GenericEvent(string eventName, string pathway, string payload)
  {
    EventName = eventName;
    Pathway = pathway;
    Payload = payload;
    Id = Guid.NewGuid();
  }

  public Guid Id { get; set; } //Unique = Guid.NewGuid();

  public string EventName { get; set; } // The type of event (e.g., ParticipantInvited)
  public string Pathway { get; set; }  // The pathway name (e.g., Cancer Screening)

  public string Topic { get; set; } // The message topic, which correlates to the 'queue' the message is sent to
  public string NhsNumber { get; set; }
  public string Payload { get; set; } // Additional data specific to the event
}
