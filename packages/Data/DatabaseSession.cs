using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IDatabaseSession
{
    ISessionRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    ISessionRepository<Video, Guid> Videos { get; }
    ISessionRepository<VideoFrame, Guid> VideoFrames { get; }
    ISessionRepository<VideoFrameRecognition, Guid> VideoFrameRecognitions { get; }
}

public class DatabaseSession(Session session) : IDatabaseSession
{
    public ISessionRepository<MovieRecognition, Guid> MovieRecognitions { get; } =
        new MovieRecognitionSessionRepository(session);

    public ISessionRepository<Video, Guid> Videos { get; } = new VideoSessionRepository(session);
    public ISessionRepository<VideoFrame, Guid> VideoFrames { get; } = new VideoFrameSessionRepository(session);

    public ISessionRepository<VideoFrameRecognition, Guid> VideoFrameRecognitions { get; } =
        new VideoFrameRecognitionSessionRepository(session);
}