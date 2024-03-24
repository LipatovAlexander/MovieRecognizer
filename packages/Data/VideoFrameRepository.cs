using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IVideoFrameRepository : IRepository<VideoFrame, Guid>
{
    Task<IReadOnlyCollection<VideoFrame>> ListAsync(Guid videoId);
}

public class VideoFrameRepository(
    IDatabaseContext databaseContext,
    Func<IDatabaseSession, ISessionRepository<VideoFrame, Guid>> sessionRepositoryProvider)
    : Repository<VideoFrame, Guid>(databaseContext, sessionRepositoryProvider), IVideoFrameRepository
{
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<IReadOnlyCollection<VideoFrame>> ListAsync(Guid videoId)
    {
        return await _databaseContext.ExecuteAsync(async session =>
        {
            var (videoFrames, _) = await session.VideoFrames.ListAsync(
                videoId,
                TxControl.BeginSerializableRW().Commit());

            return videoFrames;
        });
    }
}