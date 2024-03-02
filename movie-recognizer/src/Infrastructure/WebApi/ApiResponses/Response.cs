namespace Infrastructure.WebApi.ApiResponses;

public abstract class Response(bool ok)
{
    public bool Ok { get; } = ok;
}
