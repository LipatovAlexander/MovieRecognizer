using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IDatabaseSession
{
    ISessionRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    ISessionRepository<Video, Guid> Videos { get; }
    IVideoFrameSessionRepository VideoFrames { get; }
    IVideoFrameRecognitionSessionRepository VideoFrameRecognitions { get; }
}

public class DatabaseSession(Session session) : IDatabaseSession
{
    public ISessionRepository<MovieRecognition, Guid> MovieRecognitions { get; } =
        new MovieRecognitionSessionRepository(session);

    public ISessionRepository<Video, Guid> Videos { get; } = new VideoSessionRepository(session);
    public IVideoFrameSessionRepository VideoFrames { get; } = new VideoFrameSessionRepository(session);

    public IVideoFrameRecognitionSessionRepository VideoFrameRecognitions { get; } =
        new VideoFrameRecognitionSessionRepository(session);
}