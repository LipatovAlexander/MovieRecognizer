using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data.Repositories;

public class VideoSessionRepository(Session session) : ISessionRepository<Video, Guid>
{
    private readonly Session _session = session;

    public async Task<(Video?, Transaction?)> TryGetAsync(
        Guid id,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;

                             SELECT *
                             FROM video
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
        var movieRecognitionId = Guid.Parse(row["movie_recognition_id"].GetUtf8());
        var externalId = row["external_id"].GetUtf8();
        var title = row["title"].GetUtf8();
        var author = row["author"].GetUtf8();
        var duration = row["duration"].GetInterval();

        var video = new Video(movieRecognitionId, externalId, title, author, duration)
        {
            Id = returnedId
        };

        return (video, response.Tx);
    }

    public async Task<Transaction?> SaveAsync(Video video, TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;
                             DECLARE $movie_recognition_id AS Utf8;
                             DECLARE $external_id AS Utf8;
                             DECLARE $title AS Utf8;
                             DECLARE $author AS Utf8;
                             DECLARE $duration AS Interval;

                             UPSERT INTO video(id, movie_recognition_id, external_id, title, author, duration)
                             VALUES ($id, $movie_recognition_id, $external_id, $title, $author, $duration);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(video.Id.ToString()),
            ["$movie_recognition_id"] = YdbValue.MakeUtf8(video.MovieRecognitionId.ToString()),
            ["$external_id"] = YdbValue.MakeUtf8(video.ExternalId),
            ["$title"] = YdbValue.MakeUtf8(video.Title),
            ["$author"] = YdbValue.MakeUtf8(video.Author),
            ["$duration"] = YdbValue.MakeInterval(video.Duration)
        };

        var response = await _session.ExecuteDataQuery(
            query,
            txControl,
            parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }
}