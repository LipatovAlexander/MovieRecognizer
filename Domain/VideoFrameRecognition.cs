namespace Domain;

public class VideoFrameRecognition(Guid videoFrameId, IReadOnlyCollection<RecognizedTitle> recognizedTitles)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoFrameId { get; set; } = videoFrameId;
    public IReadOnlyCollection<RecognizedTitle> RecognizedTitles { get; set; } = recognizedTitles;
}

public class RecognizedTitle(string title, string subtitle, string description, Uri link, Uri thumbnail)
{
    public string Title { get; set; } = title;
    public string Subtitle { get; set; } = subtitle;
    public string Description { get; set; } = description;
    public Uri Link { get; set; } = link;
    public Uri Thumbnail { get; set; } = thumbnail;
}