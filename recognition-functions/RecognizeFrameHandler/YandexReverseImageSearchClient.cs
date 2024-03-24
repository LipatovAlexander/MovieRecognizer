using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace RecognizeFrameHandler;

public interface IYandexReverseImageSearchClient
{
    Task<YandexReverseImageSearchResponse> SearchAsync(YandexReverseImageSearchRequest request,
        CancellationToken cancellationToken = default);
}

public class YandexReverseImageSearchClient(
    HttpClient httpClient,
    IOptions<YandexReverseImageSearchSettings> settings) : IYandexReverseImageSearchClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<YandexReverseImageSearchSettings> _settings = settings;

    public async Task<YandexReverseImageSearchResponse> SearchAsync(
        YandexReverseImageSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, _settings.Value.Url)
        {
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Api-Key", _settings.Value.ApiKey)
            },
            Content = JsonContent.Create(request)
        };

        var responseMessage = await _httpClient.SendAsync(message, cancellationToken);

        responseMessage.EnsureSuccessStatusCode();

        var response =
            await responseMessage.Content.ReadFromJsonAsync<YandexReverseImageSearchResponse>(cancellationToken);

        return response!;
    }
}

public class YandexReverseImageSearchSettings
{
    [Required] public required Uri Url { get; set; }
    [Required] public required string ApiKey { get; set; }
}

public class YandexReverseImageSearchRequest
{
    public required Uri ImageUrl { get; set; }
}

public class YandexReverseImageSearchResponse
{
    [JsonPropertyName("knowledge_graph")]
    public required IReadOnlyCollection<YandexReverseImageSearchKnowledgeGraphItem> KnowledgeGraph { get; set; }
}

public class YandexReverseImageSearchKnowledgeGraphItem
{
    [JsonPropertyName("title")] public required string Title { get; set; }

    [JsonPropertyName("subtitle")] public required string Subtitle { get; set; }

    [JsonPropertyName("description")] public required string Description { get; set; }

    [JsonPropertyName("link")] public required Uri Link { get; set; }

    [JsonPropertyName("source")] public required string Source { get; set; }

    [JsonPropertyName("thumbnail")] public required Uri Thumbnail { get; set; }
}