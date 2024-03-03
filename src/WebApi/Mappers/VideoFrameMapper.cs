using Domain.Entities;
using WebApi.Models;

namespace WebApi.Mappers;

public static class VideoFrameMapper
{
    public static VideoFrameDto ToDto(this VideoFrame videoFrame)
    {
        return new VideoFrameDto
        {
            Id = videoFrame.Id,
            ExternalId = videoFrame.ExternalId,
            Timestamp = videoFrame.Timestamp
        };
    }
}