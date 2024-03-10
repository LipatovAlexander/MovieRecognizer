namespace Domain;

public class MovieRecognition(Guid id, Uri videoUrl, MovieRecognitionStatus status, DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public Uri VideoUrl { get; set; } = videoUrl;
    public MovieRecognitionStatus Status { get; set; } = status;
    public DateTime CreatedAt { get; set; } = createdAt;
}

public enum MovieRecognitionStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded,
    Cancelled
}