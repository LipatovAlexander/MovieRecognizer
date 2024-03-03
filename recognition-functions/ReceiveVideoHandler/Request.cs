using System.Text.Json.Serialization;

namespace ReceiveVideoHandler;

public class Request
{
    [JsonPropertyName("httpMethod")]
    public required string HttpMethod { get; set; }

    [JsonPropertyName("body")]
    public required string Body { get; set; }
}