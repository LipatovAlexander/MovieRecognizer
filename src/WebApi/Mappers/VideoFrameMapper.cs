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
            Timestamp = videoFrame.Timestamp,
            File = videoFrame.File.ToDto()
        };
    }
}