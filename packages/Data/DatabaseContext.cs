using Data.Repositories;
using Domain;
using Ydb.Sdk;
using Ydb.Sdk.Services.Query;

namespace Data;

public interface IDatabaseContext
{
    IMovieRecognitionRepository MovieRecognitions { get; }
    IRepository<Video, Guid> Videos { get; }
    IVideoFrameRepository VideoFrames { get; }
    IVideoFrameRecognitionRepository VideoFrameRecognitions { get; }

    Task<TResponse> ExecuteAsync<TResponse>(Func<IDatabaseSession, Task<TResponse>> execFunc);
    Task ExecuteAsync(Func<IDatabaseSession, Task> execFunc);
}

public class DatabaseContext : IDatabaseContext
{
    private readonly IYandexDbService _yandexDbService;

    public IMovieRecognitionRepository MovieRecognitions { get; }
    public IRepository<Video, Guid> Videos { get; }
    public IVideoFrameRepository VideoFrames { get; }
    public IVideoFrameRecognitionRepository VideoFrameRecognitions { get; }

    public DatabaseContext(IYandexDbService yandexDbService)
    {
        _yandexDbService = yandexDbService;

        MovieRecognitions = new MovieRecognitionRepository(this, session => session.MovieRecognitions);
        Videos = new Repository<Video, Guid>(this, session => session.Videos);
        VideoFrames = new VideoFrameRepository(this, session => session.VideoFrames);
        VideoFrameRecognitions = new VideoFrameRecognitionRepository(this, session => session.VideoFrameRecognitions);
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