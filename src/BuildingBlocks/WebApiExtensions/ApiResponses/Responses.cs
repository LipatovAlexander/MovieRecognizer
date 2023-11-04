namespace WebApiExtensions.ApiResponses;

public static class Responses
{
    public static SuccessResponse Success() => new();
    public static SuccessResponse<TValue> Success<TValue>(TValue value) => new(value);
    public static ErrorResponse Error(string code, string[]? details = null) => new(code, details);
}
