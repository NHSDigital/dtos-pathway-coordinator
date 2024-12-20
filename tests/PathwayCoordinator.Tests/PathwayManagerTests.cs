using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PathwayCoordinator.Models;
using PathwayCoordinator.PathwayManager.Steps;

namespace PathwayCoordinator.Tests;

public class PathwayManagerTests
{
  [Fact]
  public async Task ExecuteStepsAsync_ShouldCallCorrectSteps()
  {
    // Arrange
    var mockLogger = new Mock<ILogger<AddParticipantToPathway>>();
    var mockPathwayLogger = new Mock<ILogger<PathwayManager.PathwayManager>>();
    var services = new ServiceCollection();
    services.AddScoped<AddParticipantToPathway>(_ => new AddParticipantToPathway(mockLogger.Object));
    var serviceProvider = services.BuildServiceProvider();

    var pathwayManager = new PathwayManager.PathwayManager(mockPathwayLogger.Object, serviceProvider);

    Pathway pathway = new Pathway("Breast Regular", new List<PathwayStep>
    {
      new PathwayStep("AddParticipantToPathway", "ParticipantInvited", "{ \"NHSNumber\" : \"12342323\" }")
    });

    var testEvent = new GenericEvent("ParticipantInvited", "Breast Regular", "{ \"NHSNumber\" : \"1234567890\" }");
    // Act
    await pathwayManager.ExecuteStepsAsync(pathway, testEvent);

    mockLogger.Verify(
      x => x.Log(
        LogLevel.Information,                          // The log level to check
        It.IsAny<EventId>(),                           // Ignore EventId
        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding participant to pathway")),
        null,                                          // Ignore the exception
        It.IsAny<Func<It.IsAnyType, Exception, string>>() // Ignore the formatter
      )
    );

  }
}
