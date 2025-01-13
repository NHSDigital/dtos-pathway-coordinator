using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.Configure<EventGridSettings>(builder.Configuration.GetSection("EventGrid"));
builder.Services.AddSingleton<EventGridPublisherClient>(sp =>
{
  var config = sp.GetRequiredService<IOptions<EventGridSettings>>().Value;
  return new EventGridPublisherClient(new Uri(config.Endpoint), new AzureKeyCredential(config.Key));
});



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
  public string Endpoint { get; set; }
  public string Key { get; set; }
}
