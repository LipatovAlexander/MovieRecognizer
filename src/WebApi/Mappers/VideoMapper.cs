using Domain.Entities;
using WebApi.Models;

namespace WebApi.Mappers;

public static class VideoMapper
{
    public static VideoDto ToDto(this Video video)
    {
        return new VideoDto
        {
            Id = video.Id,
            ExternalId = video.ExternalId,
            Author = video.Author,
            Duration = video.Duration,
            Title = video.Title,
            VideoFrames = video.VideoFrames.Select(VideoFrameMapper.ToDto).ToArray(),
        };
    }
}