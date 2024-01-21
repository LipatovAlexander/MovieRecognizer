using System.Text.Json.Serialization;

namespace SerpApi;

public abstract class SerpSearchMetadata
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("json_endpoint")]
    public Uri JsonEndpoint { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("processed_at")]
    public DateTimeOffset ProcessedAt { get; set; }

    [JsonPropertyName("raw_html_file")]
    public Uri RawHtmlFile { get; set; }

    [JsonPropertyName("total_time_taken")]
    public double TotalTimeTaken { get; set; }
}

public abstract class SerpSearchParameters
{
    [JsonPropertyName("engine")]
    public string Engine { get; set; }
}

public abstract class SerpSearchResponse<TSearchMetadata, TSearchParameters>
    where TSearchMetadata : SerpSearchMetadata
    where TSearchParameters : SerpSearchParameters
{
    [JsonPropertyName("search_metadata")]
    public TSearchMetadata SearchMetadata { get; set; }
    
    [JsonPropertyName("search_parameters")]
    public TSearchParameters SearchParameters { get; set; }
}