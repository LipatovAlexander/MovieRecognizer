using System.Text.Json.Serialization;

namespace SerpApi;

public class YandexReverseImageSearchMetadata : SerpSearchMetadata
{
    [JsonPropertyName("yandex_images_url")]
    public Uri YandexImagesUrl { get; set; }
}

public class YandexReverseImageSearchParameters : SerpSearchParameters
{
    [JsonPropertyName("url")]
    public Uri Url { get; set; }
}

public class YandexReverseImageSearchKnowledgeGraphItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("link")]
    public Uri Link { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; }
}

public class YandexReverseImageSearchImageTag
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("link")]
    public Uri Link { get; set; }
    
    [JsonPropertyName("serpapi_link")]
    public Uri SerpApiLink { get; set; }
}

public class YandexReverseImageSearchImageResult
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("link")]
    public Uri Link { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }
}

public class YandexReverseImageSearchResponse : SerpSearchResponse<YandexReverseImageSearchMetadata, YandexReverseImageSearchParameters>
{
    [JsonPropertyName("knowledge_graph")]
    public YandexReverseImageSearchKnowledgeGraphItem[] KnowledgeGraph { get; set; }

    [JsonPropertyName("image_tags")]
    public YandexReverseImageSearchImageTag[] ImageTags { get; set; }

    [JsonPropertyName("image_results")]
    public YandexReverseImageSearchImageResult ImageResults { get; set; }
}