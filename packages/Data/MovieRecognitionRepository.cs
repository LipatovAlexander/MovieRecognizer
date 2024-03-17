using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public class MovieRecognitionRepository(Session session) : IRepository<MovieRecognition, Guid>
{
    private readonly Session _session = session;

    public async Task<(MovieRecognition?, Transaction?)> TryGetAsync(
        Guid id,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;

                             SELECT *
                             FROM `movie-recognition`
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
        var videoUrl = new Uri(row["video_url"].GetUtf8());
        var status = Enum.Parse<MovieRecognitionStatus>(row["status"].GetUtf8());
        var createdAt = row["created_at"].GetDatetime();
        var rawVideoId = row["video_id"].GetOptionalUtf8();
        var videoId = rawVideoId is null ? null as Guid? : Guid.Parse(rawVideoId);

        var movieRecognition = new MovieRecognition(videoUrl)
        {
            Id = returnedId,
            Status = status,
            CreatedAt = createdAt,
            VideoId = videoId
        };

        return (movieRecognition, response.Tx);
    }

    public async Task<Transaction?> SaveAsync(
        MovieRecognition entity,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;
                             DECLARE $video_url AS Utf8;
                             DECLARE $created_at AS Datetime;
                             DECLARE $status AS Utf8;
                             DECLARE $video_id AS Utf8?;

                             UPSERT INTO `movie-recognition`(id, video_url, created_at, status, video_id)
                             VALUES ($id, $video_url, $created_at, $status, $video_id);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
            ["$video_url"] = YdbValue.MakeUtf8(entity.VideoUrl.ToString()),
            ["$created_at"] = YdbValue.MakeDatetime(entity.CreatedAt),
            ["$status"] = YdbValue.MakeUtf8(entity.Status.ToString()),
            ["$video_id"] = YdbValue.MakeOptionalUtf8(entity.VideoId?.ToString())
        };

        var response = await _session.ExecuteDataQuery(query, txControl, parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }
}