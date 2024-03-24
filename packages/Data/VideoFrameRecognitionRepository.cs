using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IVideoFrameRecognitionRepository : IRepository<VideoFrameRecognition, Guid>
{
    Task<IReadOnlyCollection<VideoFrameRecognition>> ListAsync(Guid videoFrameId);
}

public class VideoFrameRecognitionRepository(
    IDatabaseContext databaseContext,
    Func<IDatabaseSession, ISessionRepository<VideoFrameRecognition, Guid>> sessionRepositoryProvider)
    : Repository<VideoFrameRecognition, Guid>(databaseContext, sessionRepositoryProvider),
        IVideoFrameRecognitionRepository
{
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<IReadOnlyCollection<VideoFrameRecognition>> ListAsync(Guid videoFrameId)
    {
        return await _databaseContext.ExecuteAsync(async session =>
        {
            var (videoFrameRecognitions, _) = await session.VideoFrameRecognitions.ListAsync(
                videoFrameId,
                TxControl.BeginSerializableRW().Commit());

            return videoFrameRecognitions;
        });
    }
}