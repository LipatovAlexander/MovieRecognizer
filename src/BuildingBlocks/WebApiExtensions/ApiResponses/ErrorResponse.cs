namespace WebApiExtensions.ApiResponses;

public class ErrorResponse(string code, string[]? details) : Response(false)
{
    public string Code { get; } = code;
    public string[]? Details { get; } = details;
}
