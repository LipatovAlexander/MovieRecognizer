namespace Domain;

public class MovieRecognition(Guid userId, Uri videoUrl)
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public Guid UserId { get; } = userId;
	public Uri VideoUrl { get; } = videoUrl;
	public MovieRecognitionStatus Status { get; set; } = MovieRecognitionStatus.Created;
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
	public Guid? VideoId { get; set; }
	public RecognizedTitle? RecognizedMovie { get; set; }
	public string? FailureMessage { get; set; }
	public bool? RecognizedCorrectly { get; set; }
}

public enum MovieRecognitionStatus
{
	Created,
	InProgress,
	Failed,
	Succeeded,
	Invalid
}