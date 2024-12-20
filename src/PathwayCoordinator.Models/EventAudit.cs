namespace PathwayCoordinator.Models;

public class EventAudit()
{

  public EventAudit(GenericEvent? genericEvent) : this()
  {
    NHSNumber = "12345";
    EventName = genericEvent.TriggerEvent;
    Pathway = genericEvent.Pathway;
    NextAction = "My Next Action";
  }
  public Guid Id { get; set; } = Guid.NewGuid();
  public string NHSNumber { get; set; }
  public string EventName { get; set; }
  public string Pathway { get; set; }
  public string NextAction { get; set; }
  public DateTime Timestamp { get; set; }
  public TimeSpan Duration { get; set; }
}
