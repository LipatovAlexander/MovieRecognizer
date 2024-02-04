using System.Text.Json.Serialization;

namespace SerpApi;

public class YandexReverseImageSearchMetadata : SerpSearchMetadata
{
    [JsonPropertyName("yandex_images_url")]
    public Uri YandexImagesUrl { get; set; } = null!;
}

public class YandexReverseImageSearchParameters : SerpSearchParameters
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; } = null!;
}

public class YandexReverseImageSearchKnowledgeGraphItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("link")]
    public Uri Link { get; set; } = null!;

    [JsonPropertyName("source")]
    public string Source { get; set; } = null!;

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = null!;
}

public class YandexReverseImageSearchImageTag
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("link")]
    public Uri Link { get; set; } = null!;

    [JsonPropertyName("serpapi_link")]
    public Uri SerpApiLink { get; set; } = null!;
}

public class YandexReverseImageSearchImageResult
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("link")]
    public Uri Link { get; set; } = null!;

    [JsonPropertyName("source")]
    public string Source { get; set; } = null!;
}

public class YandexReverseImageSearchResponse : SerpSearchResponse<YandexReverseImageSearchMetadata, YandexReverseImageSearchParameters>
{
    [JsonPropertyName("knowledge_graph")]
    public YandexReverseImageSearchKnowledgeGraphItem[] KnowledgeGraph { get; set; } = null!;

    [JsonPropertyName("image_tags")]
    public YandexReverseImageSearchImageTag[] ImageTags { get; set; } = null!;

    [JsonPropertyName("image_results")]
    public YandexReverseImageSearchImageResult ImageResults { get; set; } = null!;
}