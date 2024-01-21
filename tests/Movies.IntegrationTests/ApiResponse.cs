namespace Movies.IntegrationTests;

public class ApiResponse
{
    public bool Ok { get; set; }
    public string? Code { get; set; }
    public string[]? Details { get; set; }
}

public class ApiResponse<TValue> : ApiResponse
{
    public TValue? Value { get; set; }
}
