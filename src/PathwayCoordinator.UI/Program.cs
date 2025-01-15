using Microsoft.Extensions.Options;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Models;
using Shared.Clients.Clients;
using Shared.Clients.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddHttpClient<IPathwayApiClient, PathwayApiClient>((sp, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["PathwayApiClient:BaseAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

app.Run();
