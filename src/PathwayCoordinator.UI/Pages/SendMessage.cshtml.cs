using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc.Rendering;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;

namespace PathwayCoordinator.UI.Pages
{

  using System.Collections.Generic;
  public class SendModel : PageModel
  {

    [BindProperty]
    public GenericEvent Event { get; set; } = new();
    [BindProperty]
    public List<Pathway> Pathways { get; set; } = new();
    [BindProperty]
    public List<PathwayStep> AvailableSteps { get; set; } = new();

    public string PayloadTemplate { get; set; }

    private readonly IConfiguration _configuration;
    private readonly IPathwayApiClient _apiClient;

    public SendModel(IConfiguration configuration, IPathwayApiClient apiClient)
    {
      _configuration = configuration;
      _apiClient = apiClient;
    }

    public async Task OnGetAsync()
    {
      Pathways = await _apiClient.GetPathwaysAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      //If we have a Selected Pathway, but nothing else we need to populate the rest of the objects
      Pathways = await _apiClient.GetPathwaysAsync();
      if (!string.IsNullOrEmpty(Event.Pathway) &&
          (string.IsNullOrEmpty(Event.TriggerEvent) || string.IsNullOrEmpty(Event.Payload)))
      {
        var selectedPathway = Pathways.FirstOrDefault(p => p.Name == Event.Pathway);
        AvailableSteps = selectedPathway?.Steps ?? new List<PathwayStep>();
        PayloadTemplate = "";
      }

      if (!string.IsNullOrEmpty(Event.Pathway) &&
          (!string.IsNullOrEmpty(Event.TriggerEvent) && string.IsNullOrEmpty(Event.Payload)))
      {
        var selectedPathway = Pathways.FirstOrDefault(p => p.Name == Event.Pathway);
        var selectedStep = selectedPathway?.Steps.FirstOrDefault(s => s.TriggerEvent == Event.TriggerEvent);
        PayloadTemplate = selectedStep.MessageTemplate.ToString();
      }

      // Validate and process the form submission
      if (string.IsNullOrEmpty(Event.Pathway) ||
          string.IsNullOrEmpty(Event.TriggerEvent) ||
          string.IsNullOrEmpty(Event.Payload))
      {
        ModelState.AddModelError(string.Empty, "All fields are required.");
        return Page();
      }
      var client = new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnection"));

      var queueSender = client.CreateSender("participant-events-topic");
      var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(Event));

      await queueSender.SendMessageAsync(serviceBusMessage);

      // Return to the page after submission
      return RedirectToPage();
    }
  }
}
