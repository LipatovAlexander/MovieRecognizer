using System.Text.Json;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public class MovieRecognitionSessionRepository(Session session) : ISessionRepository<MovieRecognition, Guid>
{
    private readonly Session _session = session;

    public async Task<(MovieRecognition?, Transaction?)> TryGetAsync(
        Guid id,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;

                             SELECT *
                             FROM movie_recognition
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
        var userId = Guid.Parse(row["user_id"].GetUtf8());
        var videoUrl = new Uri(row["video_url"].GetUtf8());
        var status = Enum.Parse<MovieRecognitionStatus>(row["status"].GetUtf8());
        var createdAt = row["created_at"].GetDatetime();
        var rawVideoId = row["video_id"].GetOptionalUtf8();
        var videoId = rawVideoId is null ? null as Guid? : Guid.Parse(rawVideoId);
        var recognizedMovieJson = row["recognized_movie"].GetOptionalJson();
        var recognizedMovie = recognizedMovieJson is not null
            ? JsonSerializer.Deserialize<RecognizedTitle>(recognizedMovieJson)
            : null;
        var failureMessage = row["failure_message"].GetOptionalUtf8();

        var movieRecognition = new MovieRecognition(userId, videoUrl)
        {
            Id = returnedId,
            Status = status,
            CreatedAt = createdAt,
            VideoId = videoId,
            RecognizedMovie = recognizedMovie,
            FailureMessage = failureMessage
        };

        return (movieRecognition, response.Tx);
    }

    public async Task<Transaction?> SaveAsync(
        MovieRecognition entity,
        TxControl txControl)
    {
        const string query = """
                             DECLARE $id AS Utf8;
                             DECLARE $user_id AS Utf8;
                             DECLARE $video_url AS Utf8;
                             DECLARE $created_at AS Datetime;
                             DECLARE $status AS Utf8;
                             DECLARE $video_id AS Utf8?;
                             DECLARE $failure_message AS Utf8?;
                             DECLARE $recognized_movie AS Json?;

                             UPSERT INTO `movie-recognition`(id, user_id, video_url, created_at, status, video_id, failure_message, recognized_movie)
                             VALUES ($id, $user_id, $video_url, $created_at, $status, $video_id, $failure_message, $recognized_movie);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
            ["$user_id"] = YdbValue.MakeUtf8(entity.UserId.ToString()),
            ["$video_url"] = YdbValue.MakeUtf8(entity.VideoUrl.ToString()),
            ["$created_at"] = YdbValue.MakeDatetime(entity.CreatedAt),
            ["$status"] = YdbValue.MakeUtf8(entity.Status.ToString()),
            ["$video_id"] = YdbValue.MakeOptionalUtf8(entity.VideoId?.ToString()),
            ["$recognized_movie"] = YdbValue.MakeOptionalJson(
                entity.RecognizedMovie is not null
                    ? JsonSerializer.Serialize(entity.RecognizedMovie)
                    : null),
            ["$failure_message"] = YdbValue.MakeOptionalUtf8(entity.FailureMessage)
        };

        var response = await _session.ExecuteDataQuery(query, txControl, parameters);

        response.Status.EnsureSuccess();

        return response.Tx;
    }
}