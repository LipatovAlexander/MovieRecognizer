using Api.Models;
using Domain;

namespace Api.Mappers;

public static class VideoFrameMapper
{
    public static VideoFrameDto ToDto(this VideoFrame videoFrame, Uri fileUrl)
    {
        return new VideoFrameDto
        {
            Timestamp = videoFrame.Timestamp,
            FileUrl = fileUrl,
            Processed = videoFrame.Processed
        };
    }
}