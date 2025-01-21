namespace ContextManager.API.Models;

public class Participant
{
    public string NhsNumber { get; set; }
    public List<Pathway> Pathways { get; set; }
}