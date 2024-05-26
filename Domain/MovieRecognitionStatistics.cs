namespace Domain;

public record MovieRecognitionStatistics(
	ulong TotalRecognitions,
	ulong TotalRecognized,
	ulong CorrectlyRecognized,
	ulong IncorrectlyRecognized);