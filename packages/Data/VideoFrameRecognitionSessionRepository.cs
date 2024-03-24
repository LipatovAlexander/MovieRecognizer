using System.Text.Json;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public class VideoFrameRecognitionSessionRepository(Session session) : ISessionRepository<VideoFrameRecognition, Guid>
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
        var videoFrameId = Guid.Parse(row["video_frame_id"].GetUtf8());
        var recognizedTitlesJson = row["recognized_titles"].GetJson();
        var recognizedTitles = JsonSerializer.Deserialize<IReadOnlyCollection<RecognizedTitle>>(recognizedTitlesJson)!;

        var videoFrameRecognition = new VideoFrameRecognition(videoFrameId, recognizedTitles)
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
                             DECLARE $recognized_titles AS Json;

                             UPSERT INTO video_frame_recognition(id, video_frame_id, recognized_titles)
                             VALUES ($id, $video_frame_id, $recognized_titles);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
            ["$video_frame_id"] = YdbValue.MakeUtf8(entity.VideoFrameId.ToString()),
            ["$recognized_titles"] = YdbValue.MakeJson(JsonSerializer.Serialize(entity.RecognizedTitles))
        };

        var response = await _session.ExecuteDataQuery(
            query,
            txControl,
            parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }
}