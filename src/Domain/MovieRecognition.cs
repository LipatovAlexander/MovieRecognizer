namespace Domain;

public class MovieRecognition
{
    public MovieRecognition(Uri videoUrl) : this(videoUrl, DateTimeOffset.UtcNow, MovieRecognitionStatus.Created)
    {
    }
    
    public MovieRecognition(Uri videoUrl, DateTimeOffset createdAt, MovieRecognitionStatus status)
    {
        VideoUrl = videoUrl;
        CreatedAt = createdAt;
        Status = status;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public Uri VideoUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public MovieRecognitionStatus Status { get; set; }
}

public enum MovieRecognitionStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded
}