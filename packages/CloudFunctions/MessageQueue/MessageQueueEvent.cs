using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueEvent
{
    [JsonPropertyName("messages")] public required IReadOnlyList<MessageQueueMessage> Messages { get; set; }
}