using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IDatabaseSession
{
    IMovieRecognitionRepository MovieRecognitions { get; }
    IRepository<Video, Guid> Videos { get; }
}

public class DatabaseSession(Session session) : IDatabaseSession
{
    public IMovieRecognitionRepository MovieRecognitions { get; } = new MovieRecognitionRepository(session);
    public IRepository<Video, Guid> Videos { get; } = new VideoRepository(session);
}