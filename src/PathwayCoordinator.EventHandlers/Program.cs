using System.Configuration;
using ContextManager.API.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using PathwayCoordinator.PathwayManager;
using PathwayCoordinator.PathwayManager.Steps;
using Shared.Clients.Clients;
using Shared.Clients.Interfaces;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
// Add services to the container.

builder.Services.AddHttpClient<IPathwayApiClient, PathwayApiClient>((sp, client) =>
{
  client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiClients:PathwayApiClientUri"));
});

builder.Services.AddHttpClient<IAuditApiClient, AuditApiClient>((sp, client) =>
{
  client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiClients:AuditApiClientUri"));
});

builder.Services.AddHttpClient<IContextManagerService, ContextManagerApiClient>((sp, client) =>
{
  client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiClients:ContextManagerApiClientUri"));
});

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
  .AddScoped<MockProduct>()
  .AddApplicationInsightsTelemetryWorkerService()
  .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
