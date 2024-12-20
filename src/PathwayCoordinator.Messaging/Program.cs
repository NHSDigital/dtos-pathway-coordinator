using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.PathwayManager;
using PathwayCoordinator.PathwayManager.Steps;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services
  .AddLogging(logging =>
  {
    logging.AddConsole();
  })
// Register PathwayManager
  .AddScoped<IPathwayManager, PathwayManager>()
  .AddScoped<AddParticipantToPathway>()
  .AddScoped<CreateParticipantEpisode>()
  .AddScoped<GetParticipantDetails>()
  .AddScoped<UpdateParticipantPathwayStatus>()
  .AddScoped<IPathwayApiClient, PathwayApiClient>()
  .AddApplicationInsightsTelemetryWorkerService()
  .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
