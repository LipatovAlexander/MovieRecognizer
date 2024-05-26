using Api.Models;
using Data.Models;

namespace Api.Mappers;

public static class TopRecognizedTitleMapper
{
	public static TopRecognizedTitleDto ToDto(this TopRecognizedTitle topRecognizedTitle)
	{
		return new TopRecognizedTitleDto
		{
			RecognizedTitle = topRecognizedTitle.RecognizedTitle.ToDto(),
			Count = topRecognizedTitle.Count
		};
	}
}