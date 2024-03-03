using System.Text.Json.Serialization;

namespace Application.Images;

public class ImageSearchResponse
{
    [JsonPropertyName("knowledge_graph")]
    public IReadOnlyCollection<KnowledgeGraphItem> KnowledgeGraph { get; set; } = null!;
}

public class KnowledgeGraphItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;

    [JsonPropertyName("source")]
    public string Source { get; set; } = null!;

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = null!;
}