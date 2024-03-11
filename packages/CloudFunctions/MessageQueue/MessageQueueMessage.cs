using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueMessage
{
    [JsonPropertyName("event_metadata")] public required MessageQueueEventMetadata EventMetadata { get; set; }

    [JsonPropertyName("details")] public required MessageQueueDetails Details { get; set; }
}