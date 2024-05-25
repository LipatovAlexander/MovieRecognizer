using System.Text.Json.Serialization;

namespace Api.Models;

public class MovieRecognitionStatisticsDto
{
	[JsonPropertyName("total_recognized")]
	public required int TotalRecognized { get; set; }
	
	[JsonPropertyName("correctly_recognized")]
	public required int CorrectlyRecognized { get; set; }
	
	[JsonPropertyName("incorrectly_recognized")]
	public required int IncorrectlyRecognized { get; set; }
}