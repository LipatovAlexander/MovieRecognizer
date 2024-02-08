using System.Text.Json.Serialization;

namespace Application.SerpApi;

public abstract class SerpSearchMetadata
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    [JsonPropertyName("json_endpoint")]
    public Uri JsonEndpoint { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("processed_at")]
    public DateTimeOffset ProcessedAt { get; set; }

    [JsonPropertyName("raw_html_file")]
    public Uri RawHtmlFile { get; set; } = null!;

    [JsonPropertyName("total_time_taken")]
    public double TotalTimeTaken { get; set; }
}

public abstract class SerpSearchParameters
{
    [JsonPropertyName("engine")]
    public string Engine { get; set; } = null!;
}

public abstract class SerpSearchResponse<TSearchMetadata, TSearchParameters>
    where TSearchMetadata : SerpSearchMetadata
    where TSearchParameters : SerpSearchParameters
{
    [JsonPropertyName("search_metadata")]
    public TSearchMetadata SearchMetadata { get; set; } = null!;

    [JsonPropertyName("search_parameters")]
    public TSearchParameters SearchParameters { get; set; } = null!;
}