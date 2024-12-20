using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PathwayCoordinator.Interfaces;
using PathwayCoordinator.Messaging;
using PathwayCoordinator.Models;
using PathwayCoordinator.PathwayManager.Utils;

namespace PathwayCoordinator.Tests;

public class ParticipantEventHandlerTests
{
  [Fact]
  public async Task Run_ShouldCallPathwayManager_WithCorrectParameters()
  {
    var mockLogger = new Mock<ILogger<ParticipantEventHandler>>();
    var mockPathwayManager = new Mock<IPathwayManager>();
    var mockApiClient = new Mock<IPathwayApiClient>();
    var testPathways = new List<Pathway>()
    {
      new Pathway("Breast Regular", new List<PathwayStep>
      {
        new PathwayStep("AddParticipantToPathway", "ParticipantInvited", "{ NHSNumber = \"12342323\" }")
      })
    };


    mockApiClient.Setup(x => x.GetPathwaysAsync()).ReturnsAsync(testPathways);
    // Create a mock event message
    var testEvent = new GenericEvent("ParticipantInvited", "Breast Regular", "{ NHSNumber = \"1234567890\" }");
    var serializedMessage = JsonSerializer.Serialize(testEvent);
    var selectedPathway = testPathways.First();

    // Create the handler with mocks
    var handler = new ParticipantEventHandler(
      mockLogger.Object,
      mockPathwayManager.Object,
      mockApiClient.Object
    );

    // Mock FunctionContext
    var mockFunctionContext = new Mock<FunctionContext>();

    // Act
    await handler.Run(serializedMessage, mockFunctionContext.Object);
    // Assert
    mockPathwayManager.Verify(
      x => x.ExecuteStepsAsync(It.IsAny<Pathway>(), It.IsAny<GenericEvent>()),
      Times.Once
    );

    // Verify logging
    mockLogger.Verify(
      x => x.Log(
        LogLevel.Information,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) =>
          v.ToString().Contains($"Received event on participant-events queue : {testEvent.TriggerEvent}")),
        null,
        It.IsAny<Func<It.IsAnyType, Exception, string>>()
      ),
      Times.Once
    );
  }

  [Fact]
  public void PopulateTemplate_ShouldReplacePlaceholders_WhenValidJsonProvided()
  {
    // Arrange
    var templateProcessor = new TemplateProcessor();

    var template = @"
        {
          ""StepName"": ""Send Invitation"",
          ""Details"": {
            ""Email"": ""{{email}}"",
            ""ParticipantID"": ""{{participantId}}"",
            ""Message"": ""Hello {{firstName}}, your screening is scheduled for {{screeningDate}}.""
          }
        }";

    var inboundDataJson = @"
        {
          ""email"": ""participant@example.com"",
          ""participantId"": ""12345"",
          ""firstName"": ""John"",
          ""screeningDate"": ""2024-12-20""
        }";

    var expectedOutput = @"
        {
          ""StepName"": ""Send Invitation"",
          ""Details"": {
            ""Email"": ""participant@example.com"",
            ""ParticipantID"": ""12345"",
            ""Message"": ""Hello John, your screening is scheduled for 2024-12-20.""
          }
        }";

    // Act
    var result = templateProcessor.PopulateTemplate(template, inboundDataJson);

    // Assert
    Assert.Equal(expectedOutput,result);
  }

}

