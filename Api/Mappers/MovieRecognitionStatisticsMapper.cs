using Api.Models;
using Domain;

namespace Api.Mappers;

public static class MovieRecognitionStatisticsMapper
{
	public static MovieRecognitionStatisticsDto ToDto(this MovieRecognitionStatistics movieRecognitionStatistics)
	{
		return new MovieRecognitionStatisticsDto
		{
			TotalRecognized = movieRecognitionStatistics.TotalRecognized,
			CorrectlyRecognized = movieRecognitionStatistics.CorrectlyRecognized,
			IncorrectlyRecognized = movieRecognitionStatistics.IncorrectlyRecognized
		};
	}
}