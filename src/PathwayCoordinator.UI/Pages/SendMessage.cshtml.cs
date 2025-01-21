using System.Diagnostics.Tracing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc.Rendering;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Interfaces;

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
      //If all the data is populated, then we're going to submit to a queue

      if (string.IsNullOrEmpty(Event.Pathway) ||
          string.IsNullOrEmpty(Event.EventName) ||
          string.IsNullOrEmpty(Event.Payload) ||
          Event.Payload.Contains("{{"))
      {
        //We haven't got a data set yet, so we're going to return the page, but first we need to set the
        //dropdowns properly
        Pathways = await _apiClient.GetPathwaysAsync();
        AvailableSteps = Pathways.FirstOrDefault(p => p.Name == Event.Pathway)?.Steps;
        PayloadTemplate = AvailableSteps.FirstOrDefault(s => s.TriggerEvent == Event.EventName)?.MessageTemplate.ToString();
        Event.Payload = PayloadTemplate;
        Event.Topic = "PathwayCoordinator";
        return Page();
      }
      else
      {
        Event.Topic = "PathwayCoordinator";
        await _apiClient.PublishEvent(JsonSerializer.Serialize(Event));
        // Return to the page after submission
        return RedirectToPage();
      }
    }
  }
}
