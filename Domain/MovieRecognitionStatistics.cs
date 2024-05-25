namespace Domain;

public record MovieRecognitionStatistics(ulong TotalRecognized, ulong CorrectlyRecognized, ulong IncorrectlyRecognized);