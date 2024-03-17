using Ydb.Sdk;
using Ydb.Sdk.Services.Query;

namespace Data;

public interface IDatabaseContext
{
    Task<TResponse> ExecuteAsync<TResponse>(Func<IDatabaseSession, Task<TResponse>> execFunc);
    Task ExecuteAsync(Func<IDatabaseSession, Task> execFunc);
}

public class DatabaseContext(IYandexDbService yandexDbService) : IDatabaseContext
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<IDatabaseSession, Task<TResponse>> execFunc)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        TResponse response = default!;

        await tableClient.SessionExec(async session =>
        {
            var databaseSession = new DatabaseSession(session);
            response = await execFunc(databaseSession);

            return new QueryResponse(Status.Success);
        });

        return response;
    }

    public async Task ExecuteAsync(Func<IDatabaseSession, Task> execFunc)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        await tableClient.SessionExec(async session =>
        {
            var databaseSession = new DatabaseSession(session);
            await execFunc(databaseSession);

            return new QueryResponse(Status.Success);
        });
    }
}