using System.Text.Json.Serialization;

namespace Api.Models;

public class TopRecognizedTitleDto
{
	[JsonPropertyName("recognized_title")] public required RecognizedTitleDto RecognizedTitle { get; set; }

	[JsonPropertyName("count")] public required ulong Count { get; set; }
}