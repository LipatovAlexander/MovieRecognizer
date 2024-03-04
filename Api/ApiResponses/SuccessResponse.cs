namespace Api.ApiResponses;

public class SuccessResponse() : Response(true);

public class SuccessResponse<TValue>(TValue value) : SuccessResponse
{
    public TValue Value { get; } = value;
}
