namespace Domain;

public class Video(string externalId, string title, string author, TimeSpan duration)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ExternalId { get; set; } = externalId;
    public string Title { get; set; } = title;
    public string Author { get; set; } = author;
    public TimeSpan Duration { get; set; } = duration;
}