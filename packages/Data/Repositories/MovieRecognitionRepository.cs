using Data.YandexDb;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data.Repositories;

public interface IMovieRecognitionRepository
{
    Task<MovieRecognition?> GetAsync(Guid id);
    Task SaveAsync(MovieRecognition movieRecognition);
}

public class MovieRecognitionRepository(IYandexDbService yandexDbService) : IMovieRecognitionRepository
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<MovieRecognition?> GetAsync(Guid id)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
        {
            const string query = """
                                 DECLARE $id AS Utf8;

                                 SELECT
                                     id,
                                     video_url,
                                     created_at,
                                     status
                                 FROM movie-recognition
                                 WHERE id = $id;
                                 """;

            return await session.ExecuteDataQuery(
                query: query,
                txControl: TxControl.BeginSerializableRW().Commit(),
                parameters: new Dictionary<string, YdbValue>
                {
                    ["$id"] = YdbValue.MakeUtf8(id.ToString())
                }
            );
        });

        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        var row = resultSet.Rows.SingleOrDefault();

        if (row is null)
        {
            return null;
        }

        var rawId = (string)row["id"]!;
        var rawVideoUrl = (string)row["video_url"]!;
        var rawStatus = (string)row["status"]!;

        var returnedId = Guid.Parse(rawId);
        var videoUrl = new Uri(rawVideoUrl);
        var status = Enum.Parse<MovieRecognitionStatus>(rawStatus);
        var createdAt = (DateTime)row["created_at"]!;

        return new MovieRecognition(returnedId, videoUrl, status, createdAt);
    }

    public async Task SaveAsync(MovieRecognition movieRecognition)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
        {
            const string query = """
                                 DECLARE $id AS Utf8;
                                 DECLARE $video_url AS Utf8;
                                 DECLARE $created_at AS Datetime;
                                 DECLARE $status AS Utf8;

                                 INSERT INTO movie-recognition(id, video_url, created_at, status)
                                 VALUES ($id, $video_url, $created_at, $status);
                                 """;

            return await session.ExecuteDataQuery(
                query: query,
                txControl: TxControl.BeginSerializableRW().Commit(),
                parameters: new Dictionary<string, YdbValue>
                {
                    ["$id"] = YdbValue.MakeUtf8(movieRecognition.Id.ToString()),
                    ["$video_url"] = YdbValue.MakeUtf8(movieRecognition.VideoUrl.ToString()),
                    ["$created_at"] = YdbValue.MakeDatetime(movieRecognition.CreatedAt),
                    ["$status"] = YdbValue.MakeUtf8(movieRecognition.Status.ToString())
                }
            );
        });

        response.Status.EnsureSuccess();
    }
}