using Api.Models;
using Data.Models;

namespace Api.Mappers;

public static class MovieRecognitionStatisticsMapper
{
	public static MovieRecognitionStatisticsDto ToDto(this MovieRecognitionStatistics movieRecognitionStatistics)
	{
		return new MovieRecognitionStatisticsDto
		{
			TotalRecognitions = movieRecognitionStatistics.TotalRecognitions,
			TotalRecognized = movieRecognitionStatistics.TotalRecognized,
			CorrectlyRecognized = movieRecognitionStatistics.CorrectlyRecognized,
			IncorrectlyRecognized = movieRecognitionStatistics.IncorrectlyRecognized
		};
	}
}