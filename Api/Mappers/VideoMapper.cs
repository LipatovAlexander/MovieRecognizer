using Api.Models;
using Domain;

namespace Api.Mappers;

public static class VideoMapper
{
    public static VideoDto ToDto(this Video video)
    {
        return new VideoDto
        {
            Author = video.Author,
            Duration = video.Duration,
            Title = video.Title,
            ExternalId = video.ExternalId
        };
    }
}