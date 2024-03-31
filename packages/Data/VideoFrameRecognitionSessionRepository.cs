using System.Text.Json;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public interface IVideoFrameRecognitionSessionRepository : ISessionRepository<VideoFrameRecognition, Guid>
{
    Task<(IReadOnlyCollection<VideoFrameRecognition>, Transaction?)> ListByVideoIdAsync(Guid videoId,
        TxControl txControl);
}

public class VideoFrameRecognitionSessionRepository(Session session) : IVideoFrameRecognitionSessionRepository
{
    private readonly Session _session = session;

    public async Task<(VideoFrameRecognition?, Transaction?)> TryGetAsync(Guid id, TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;

                             SELECT *
                             FROM video_frame_recognition
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
        var videoFrameId = Guid.Parse(row["video_frame_id"].GetUtf8());
        var recognizedTitleJson = row["recognized_title"].GetJson();
        var recognizedTitles = JsonSerializer.Deserialize<RecognizedTitle>(recognizedTitleJson)!;

        var videoFrameRecognition = new VideoFrameRecognition(videoId, videoFrameId, recognizedTitles)
        {
            Id = returnedId
        };

        return (videoFrameRecognition, response.Tx);
    }

    public async Task<Transaction?> SaveAsync(VideoFrameRecognition entity, TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;
                             DECLARE $video_frame_id AS Utf8;
                             DECLARE $recognized_title AS Json;

                             UPSERT INTO video_frame_recognition(id, video_id, video_frame_id, recognized_title)
                             VALUES ($id, $video_id, $video_frame_id, $recognized_title);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
            ["$video_id"] = YdbValue.MakeUtf8(entity.VideoId.ToString()),
            ["$video_frame_id"] = YdbValue.MakeUtf8(entity.VideoFrameId.ToString()),
            ["$recognized_title"] = YdbValue.MakeJson(JsonSerializer.Serialize(entity.RecognizedTitle))
        };

        var response = await _session.ExecuteDataQuery(
            query,
            txControl,
            parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }

    public async Task<(IReadOnlyCollection<VideoFrameRecognition>, Transaction?)> ListByVideoIdAsync(
        Guid videoId,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $video_id AS Utf8;

                             SELECT *
                             FROM video_frame_recognition VIEW idx_video AS vfr
                             WHERE vfr.video_id = $video_id;
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$video_id"] = YdbValue.MakeUtf8(videoId.ToString())
        };

        var response = await _session.ExecuteDataQuery(query, txControl, parameters);

        response.Status.EnsureSuccess();

        var resultSet = response.Result.ResultSets[0];
        var rows = resultSet.Rows;

        var videoFrameRecognitions = rows
            .Select(row =>
            {
                var returnedId = Guid.Parse(row["id"].GetUtf8());
                var returnedVideoId = Guid.Parse(row["video_id"].GetUtf8());
                var returnedVideoFrameId = Guid.Parse(row["video_frame_id"].GetUtf8());
                var recognizedTitleJson = row["recognized_title"].GetJson();
                var recognizedTitles = JsonSerializer.Deserialize<RecognizedTitle>(recognizedTitleJson)!;

                return new VideoFrameRecognition(returnedVideoId, returnedVideoFrameId, recognizedTitles)
                {
                    Id = returnedId
                };
            })
            .ToArray();

        return (videoFrameRecognitions, response.Tx);
    }
}