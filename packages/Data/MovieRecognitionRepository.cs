using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public class MovieRecognitionRepository(IYandexDbService yandexDbService) : IRepository<MovieRecognition, Guid>
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<MovieRecognition?> GetAsync(Guid id)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
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

            return await session.ExecuteDataQuery(query, TxControl.BeginSerializableRW().Commit(), parameters);
        });

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

    public async Task SaveAsync(MovieRecognition entity)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
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

            return await session.ExecuteDataQuery(query, TxControl.BeginSerializableRW().Commit(), parameters);
        });

        response.Status.EnsureSuccess();
    }
}