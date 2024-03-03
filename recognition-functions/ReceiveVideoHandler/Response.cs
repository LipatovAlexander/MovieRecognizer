namespace ReceiveVideoHandler;

public class Response(int statusCode, string body)
{
    public int statusCode { get; set; } = statusCode;
    public string body { get; set; } = body;
}