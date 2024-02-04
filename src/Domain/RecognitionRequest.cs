namespace Domain;

public class RecognitionRequest
{
    public RecognitionRequest(Uri videoUrl, DateTimeOffset createdAt, RecognitionRequestStatus status)
    {
        VideoUrl = videoUrl;
        CreatedAt = createdAt;
        Status = status;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public Uri VideoUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public RecognitionRequestStatus Status { get; set; }
}

public enum RecognitionRequestStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded
}