using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IDatabaseSession
{
    IRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    IRepository<Video, Guid> Videos { get; }
}

public class DatabaseSession(Session session) : IDatabaseSession
{
    public IRepository<MovieRecognition, Guid> MovieRecognitions { get; } = new MovieRecognitionRepository(session);
    public IRepository<Video, Guid> Videos { get; } = new VideoRepository(session);
}