namespace Data.Models;

public record MovieRecognitionStatistics(
	ulong TotalRecognitions,
	ulong TotalRecognized,
	ulong CorrectlyRecognized,
	ulong IncorrectlyRecognized);