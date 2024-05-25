using System.Text.Json.Serialization;

namespace Api.Models;

public class MovieRecognitionStatisticsDto
{
	[JsonPropertyName("total_recognized")]
	public required ulong TotalRecognized { get; set; }
	
	[JsonPropertyName("correctly_recognized")]
	public required ulong CorrectlyRecognized { get; set; }
	
	[JsonPropertyName("incorrectly_recognized")]
	public required ulong IncorrectlyRecognized { get; set; }
}