using Api.Models;
using Domain;

namespace Api.Mappers;

public static class RecognizedTitleMapper
{
    public static RecognizedTitleDto ToDto(this RecognizedTitle recognizedTitle)
    {
        return new RecognizedTitleDto
        {
            Title = recognizedTitle.Title,
            Subtitle = recognizedTitle.Subtitle,
            Description = recognizedTitle.Description,
            Source = recognizedTitle.Source,
            Link = recognizedTitle.Link,
            Thumbnail = recognizedTitle.Thumbnail
        };
    }
}