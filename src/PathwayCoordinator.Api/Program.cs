using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PathwayCoordinator.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();


builder.Services
  .AddLogging(logging =>
  {
    logging.AddConsole();
  });

builder.Services.Configure<EventGridSettings>(
  builder.Configuration.GetSection("EventGrid"));
builder.Services.AddSingleton<EventGridClientFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});

app.Run();

public class EventGridSettings
{
  public List<EventGridTopic> Topics { get; set; }
}

public class EventGridTopic
{
  public string TopicName { get; set; }
  public string Endpoint { get; set; }
  public string Key { get; set; }
}
