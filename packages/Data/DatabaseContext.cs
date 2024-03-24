using Domain;
using Ydb.Sdk;
using Ydb.Sdk.Services.Query;

namespace Data;

public interface IDatabaseContext
{
    IRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    IRepository<Video, Guid> Videos { get; }
    IRepository<VideoFrame, Guid> VideoFrames { get; }
    IRepository<VideoFrameRecognition, Guid> VideoFrameRecognitions { get; }

    Task<TResponse> ExecuteAsync<TResponse>(Func<IDatabaseSession, Task<TResponse>> execFunc);
    Task ExecuteAsync(Func<IDatabaseSession, Task> execFunc);
}

public class DatabaseContext : IDatabaseContext
{
    private readonly IYandexDbService _yandexDbService;

    public IRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    public IRepository<Video, Guid> Videos { get; }
    public IRepository<VideoFrame, Guid> VideoFrames { get; }
    public IRepository<VideoFrameRecognition, Guid> VideoFrameRecognitions { get; }

    public DatabaseContext(IYandexDbService yandexDbService)
    {
        _yandexDbService = yandexDbService;

        MovieRecognitions = new Repository<MovieRecognition, Guid>(this, session => session.MovieRecognitions);
        Videos = new Repository<Video, Guid>(this, session => session.Videos);
        VideoFrames = new Repository<VideoFrame, Guid>(this, session => session.VideoFrames);
        VideoFrameRecognitions =
            new Repository<VideoFrameRecognition, Guid>(this, session => session.VideoFrameRecognitions);
    }

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