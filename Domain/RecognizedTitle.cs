namespace Domain;

public record RecognizedTitle(
    string Title,
    string Subtitle,
    string Description,
    string Source,
    Uri Link,
    Uri Thumbnail);