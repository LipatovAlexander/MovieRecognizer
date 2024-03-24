using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public interface IVideoFrameSessionRepository : ISessionRepository<VideoFrame, Guid>
{
    Task<(IReadOnlyCollection<VideoFrame>, Transaction?)> ListAsync(Guid videoId, TxControl txControl);
}

public class VideoFrameSessionRepository(Session session) : IVideoFrameSessionRepository
{
    private readonly Session _session = session;

    public async Task<(VideoFrame?, Transaction?)> TryGetAsync(Guid id, TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;

                             SELECT *
                             FROM video_frame
                             WHERE id = $id;
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(id.ToString())
        };

        var response = await _session.ExecuteDataQuery(query, txControl, parameters);

        response.Status.EnsureSuccess();

        var resultSet = response.Result.ResultSets[0];
        var row = resultSet.Rows.FirstOrDefault();

        if (row is null)
        {
            return (null, response.Tx);
        }

        var returnedId = Guid.Parse(row["id"].GetUtf8());
        var videoId = Guid.Parse(row["video_id"].GetUtf8());
        var timestamp = row["timestamp"].GetInterval();
        var externalId = row["external_id"].GetUtf8();

        var videoFrame = new VideoFrame(videoId, timestamp, externalId)
        {
            Id = returnedId
        };

        return (videoFrame, response.Tx);
    }

    public async Task<Transaction?> SaveAsync(VideoFrame entity, TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;
                             DECLARE $video_id AS Utf8;
                             DECLARE $timestamp AS Interval;
                             DECLARE $external_id AS Utf8;

                             UPSERT INTO video_frame(id, video_id, timestamp, external_id)
                             VALUES ($id, $video_id, $timestamp, $external_id);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
            ["$video_id"] = YdbValue.MakeUtf8(entity.VideoId.ToString()),
            ["$timestamp"] = YdbValue.MakeInterval(entity.Timestamp),
            ["$external_id"] = YdbValue.MakeUtf8(entity.ExternalId),
        };

        var response = await _session.ExecuteDataQuery(
            query,
            txControl,
            parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }

    public async Task<(IReadOnlyCollection<VideoFrame>, Transaction?)> ListAsync(Guid videoId, TxControl txControl)
    {
        const string query = """
                             DECLARE $video_id AS Utf8;

                             SELECT *
                             FROM video_frame
                             WHERE video_id = $video_id;
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$video_id"] = YdbValue.MakeUtf8(videoId.ToString())
        };

        var response = await _session.ExecuteDataQuery(query, txControl, parameters);

        response.Status.EnsureSuccess();

        var resultSet = response.Result.ResultSets[0];
        var rows = resultSet.Rows;

        var videoFrames = rows
            .Select(row =>
            {
                var id = Guid.Parse(row["id"].GetUtf8());
                var returnedVideoId = Guid.Parse(row["video_id"].GetUtf8());
                var timestamp = row["timestamp"].GetInterval();
                var externalId = row["external_id"].GetUtf8();

                return new VideoFrame(returnedVideoId, timestamp, externalId)
                {
                    Id = id
                };
            })
            .ToArray();

        return (videoFrames, response.Tx);
    }
}