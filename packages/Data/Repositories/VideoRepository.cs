using Data.YandexDb;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data.Repositories;

public class VideoRepository(IYandexDbService yandexDbService) : IVideoRepository
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<Video?> GetAsync(Guid id)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
        {
            const string query = """
                                 DECLARE $id AS Utf8;

                                 SELECT
                                     id,
                                     external_id,
                                     title,
                                     author,
                                     duration
                                 FROM `video`
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

        var returnedId = Guid.Parse((string)row["id"]!);
        var externalId = (string)row["external_id"]!;
        var title = (string)row["title"]!;
        var author = (string)row["author"]!;
        var duration = (TimeSpan)row["duration"];

        return new Video(externalId, title, author, duration)
        {
            Id = returnedId
        };
    }

    public async Task SaveAsync(Video video)
    {
        using var tableClient = _yandexDbService.GetTableClient();

        var response = await tableClient.SessionExec(async session =>
        {
            const string query = """
                                 DECLARE $id AS Utf8;
                                 DECLARE $external_id AS Utf8;
                                 DECLARE $title AS Utf8;
                                 DECLARE $author AS Utf8;
                                 DECLARE $duration AS Interval;

                                 INSERT INTO `video`(id, external_id, title, author, duration)
                                 VALUES ($id, $external_id, $title, $author, $duration);
                                 """;

            return await session.ExecuteDataQuery(
                query: query,
                txControl: TxControl.BeginSerializableRW().Commit(),
                parameters: new Dictionary<string, YdbValue>
                {
                    ["$id"] = YdbValue.MakeUtf8(video.Id.ToString()),
                    ["$external_id"] = YdbValue.MakeUtf8(video.ExternalId),
                    ["$title"] = YdbValue.MakeUtf8(video.Title),
                    ["$author"] = YdbValue.MakeUtf8(video.Author),
                    ["$duration"] = YdbValue.MakeInterval(video.Duration)
                }
            );
        });

        response.Status.EnsureSuccess();
    }
}