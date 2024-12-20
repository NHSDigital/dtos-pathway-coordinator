using Audit.Service.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PathwayCoordinator.Interfaces;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services
  .AddLogging(logging =>
  {
    logging.AddConsole();
    logging.AddDebug();
  })
  .AddApplicationInsightsTelemetryWorkerService()
  .AddScoped<IAuditApiClient, AuditApiClient>()
  .ConfigureFunctionsApplicationInsights();

// Application Insights
// isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
