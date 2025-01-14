using Azure;

namespace PathwayCoordinator.Api.Controllers;

using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Options;

public class EventGridClientFactory
{
    private readonly EventGridSettings _settings;

    public EventGridClientFactory(IOptions<EventGridSettings> options)
    {
        _settings = options.Value;
    }

    public EventGridPublisherClient CreateClient(string topicName)
    {
        // Find the configuration for the given topic name
        var topicConfig = _settings.Topics
            .FirstOrDefault(t => t.TopicName.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topicConfig == null)
        {
            throw new ArgumentException($"No configuration found for topic: {topicName}");
        }

        // Create and return the EventGridPublisherClient
        return new EventGridPublisherClient(
            new Uri(topicConfig.Endpoint),
            new AzureKeyCredential(topicConfig.Key)
        );
    }
}