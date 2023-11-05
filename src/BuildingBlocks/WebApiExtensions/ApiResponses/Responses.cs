namespace WebApiExtensions.ApiResponses;

public static class Responses
{
    public static SuccessResponse Success() => new();
    public static SuccessResponse<TValue> Success<TValue>(TValue value) => new(value);
    public static ErrorResponse Error(string code, params string[] details) => new(code, details);
}
