namespace Domain;

public class MovieRecognition(Uri videoUrl)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Uri VideoUrl { get; set; } = videoUrl;
    public MovieRecognitionStatus Status { get; set; } = MovieRecognitionStatus.Created;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? VideoId { get; set; }
    public RecognizedTitle? RecognizedMovie { get; set; }

    public string? FailureMessage { get; set; }
}

public enum MovieRecognitionStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded
}