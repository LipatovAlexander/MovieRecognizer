using System.Text.Json.Serialization;

namespace Api.Models;

public class MovieRecognitionStatisticsDto
{
	[JsonPropertyName("total_recognized")]
	public required long TotalRecognized { get; set; }
	
	[JsonPropertyName("correctly_recognized")]
	public required long CorrectlyRecognized { get; set; }
	
	[JsonPropertyName("incorrectly_recognized")]
	public required long IncorrectlyRecognized { get; set; }
}