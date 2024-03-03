using System.Text.Json.Serialization;

namespace ReceiveVideoHandler;

public class Response(int statusCode, string body, string contentType)
{
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; } = statusCode;
    
    [JsonPropertyName("body")]
    public string Body { get; set; } = body;

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new()
    {
        ["Content-Type"] = contentType
    };
}