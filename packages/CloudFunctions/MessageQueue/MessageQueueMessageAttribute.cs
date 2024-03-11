using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueMessageAttribute
{
    [JsonPropertyName("data_type")] public required string DataType { get; set; }

    [JsonPropertyName("string_value")] public required string StringValue { get; set; }
}