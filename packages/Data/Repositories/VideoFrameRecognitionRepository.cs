using Domain;
using Ydb.Sdk.Services.Table;

namespace Data.Repositories;

public interface IVideoFrameRecognitionRepository : IRepository<VideoFrameRecognition, Guid>
{
    Task<IReadOnlyCollection<VideoFrameRecognition>> ListByVideoIdAsync(Guid videoId);
}

public class VideoFrameRecognitionRepository(
    IDatabaseContext databaseContext,
    Func<IDatabaseSession, ISessionRepository<VideoFrameRecognition, Guid>> sessionRepositoryProvider)
    : Repository<VideoFrameRecognition, Guid>(databaseContext, sessionRepositoryProvider),
        IVideoFrameRecognitionRepository
{
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<IReadOnlyCollection<VideoFrameRecognition>> ListByVideoIdAsync(Guid videoId)
    {
        return await _databaseContext.ExecuteAsync(async session =>
        {
            var (videoFrameRecognitions, _) = await session.VideoFrameRecognitions.ListByVideoIdAsync(
                videoId,
                TxControl.BeginSerializableRW().Commit());

            return videoFrameRecognitions;
        });
    }
}