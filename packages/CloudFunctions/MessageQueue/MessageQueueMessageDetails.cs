using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueMessageDetails
{
    [JsonPropertyName("message_id")] public required string MessageId { get; set; }

    [JsonPropertyName("md5_of_body")] public required string Md5OfBody { get; set; }

    [JsonPropertyName("body")] public required string Body { get; set; }

    [JsonPropertyName("attributes")] public required IReadOnlyDictionary<string, string> Attributes { get; set; }

    [JsonPropertyName("message_attributes")]
    public required IReadOnlyDictionary<string, MessageQueueMessageAttribute> MessageAttributes { get; set; }

    [JsonPropertyName("md5_of_message_attributes")]
    public required string Md5OfMessageAttributes { get; set; }
}