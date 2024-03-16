using Data.YandexDb;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data.Repositories;

public class MovieRecognitionRepository(IYandexDbService yandexDbService) : IMovieRecognitionRepository
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<MovieRecognition?> GetAsync(Guid id)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        const string query = """
                             SELECT *
                             FROM `movie-recognition`
                             WHERE id = $id;
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(id.ToString())
        };

        var response = await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            query,
            TxControl.BeginSerializableRW().Commit(),
            parameters));

        response.Status.EnsureSuccess();

        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];

        var row = resultSet.Rows.FirstOrDefault();

        if (row is null)
        {
            return null;
        }

        var returnedId = Guid.Parse(row["id"].GetUtf8());
        var videoUrl = new Uri(row["video_url"].GetUtf8());
        var status = Enum.Parse<MovieRecognitionStatus>(row["status"].GetUtf8());
        var createdAt = row["created_at"].GetDatetime();
        var rawVideoId = row["video_id"].GetOptionalUtf8();
        var videoId = rawVideoId is null ? null as Guid? : Guid.Parse(rawVideoId);

        return new MovieRecognition(videoUrl)
        {
            Id = returnedId,
            Status = status,
            CreatedAt = createdAt,
            VideoId = videoId
        };
    }

    public async Task SaveAsync(MovieRecognition movieRecognition)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        const string query = """
                             UPSERT INTO `movie-recognition`(id, video_url, created_at, status, video_id)
                             VALUES ($id, $video_url, $created_at, $status, $video_id);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(movieRecognition.Id.ToString()),
            ["$video_url"] = YdbValue.MakeUtf8(movieRecognition.VideoUrl.ToString()),
            ["$created_at"] = YdbValue.MakeDatetime(movieRecognition.CreatedAt),
            ["$status"] = YdbValue.MakeUtf8(movieRecognition.Status.ToString()),
            ["$video_id"] = YdbValue.MakeOptionalUtf8(movieRecognition.VideoId?.ToString())
        };

        var response = await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            query,
            TxControl.BeginSerializableRW().Commit(),
            parameters));

        response.Status.EnsureSuccess();
    }
}