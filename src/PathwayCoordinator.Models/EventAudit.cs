namespace PathwayCoordinator.Models;

public class EventAudit()
{

  public EventAudit(GenericEvent? genericEvent) : this()
  {
    NHSNumber = "12345";
    EventName = genericEvent.EventName;
    Pathway = genericEvent.Pathway;
    NextAction = "My Next Action";
  }
  public Guid Id { get; set; } = Guid.NewGuid();
  public string NHSNumber { get; set; }
  public string EventName { get; set; }
  public string Pathway { get; set; }
  public string NextAction { get; set; }
  public DateTimeOffset Timestamp { get; set; }

  public string Version { get; set; }
  public TimeSpan Duration { get; set; }
}
