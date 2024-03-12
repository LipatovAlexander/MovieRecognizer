using Data.YandexDb;
using Domain;
using Ydb.Sdk.Value;

namespace Data.Repositories;

public class VideoRepository(IYandexDbService yandexDbService) : IVideoRepository
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    public async Task<Video?> GetAsync(Guid id)
    {
        using var queryClient = _yandexDbService.GetQueryClient();

        const string query = """
                             SELECT *
                             FROM video
                             WHERE id = $id;
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(id.ToString())
        };

        var response = await queryClient.ReadSingleRow(query, parameters);

        response.Status.EnsureSuccess();

        var row = response.Result;

        if (row is null)
        {
            return null;
        }

        var returnedId = Guid.Parse(row["id"].GetUtf8());
        var externalId = row["external_id"].GetUtf8();
        var title = row["title"].GetUtf8();
        var author = row["author"].GetUtf8();
        var duration = row["duration"].GetInterval();

        return new Video(externalId, title, author, duration)
        {
            Id = returnedId
        };
    }

    public async Task SaveAsync(Video video)
    {
        using var queryClient = _yandexDbService.GetQueryClient();

        const string query = """
                             UPSERT INTO video(id, external_id, title, author, duration)
                             VALUES ($id, $external_id, $title, $author, $duration);
                             """;

        var parameters = new Dictionary<string, YdbValue>
        {
            ["$id"] = YdbValue.MakeUtf8(video.Id.ToString()),
            ["$external_id"] = YdbValue.MakeUtf8(video.ExternalId),
            ["$title"] = YdbValue.MakeUtf8(video.Title),
            ["$author"] = YdbValue.MakeUtf8(video.Author),
            ["$duration"] = YdbValue.MakeInterval(video.Duration)
        };

        var response = await queryClient.Exec(query, parameters);

        response.Status.EnsureSuccess();
    }
}