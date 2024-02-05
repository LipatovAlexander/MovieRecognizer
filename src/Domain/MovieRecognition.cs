namespace Domain;

public class MovieRecognition(Uri videoUrl, DateTimeOffset createdAt, MovieRecognitionStatus status)
{
    public MovieRecognition(Uri videoUrl) : this(videoUrl, DateTimeOffset.UtcNow, MovieRecognitionStatus.Created)
    {
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public Uri VideoUrl { get; set; } = videoUrl;

    public DateTimeOffset CreatedAt { get; set; } = createdAt;

    public MovieRecognitionStatus Status { get; set; } = status;

    public static Specification<MovieRecognition> WithId(Guid id) => new(recognition => recognition.Id == id);
}

public enum MovieRecognitionStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded
}