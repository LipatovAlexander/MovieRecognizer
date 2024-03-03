using Domain.Entities;
using WebApi.Models;

namespace WebApi.Mappers;

public static class MovieRecognitionMapper
{
    public static MovieRecognitionDto ToDto(this MovieRecognition movieRecognition)
    {
        return new MovieRecognitionDto
        {
            Id = movieRecognition.Id,
            VideoUrl = movieRecognition.VideoUrl,
            Status = movieRecognition.Status,
            CreatedAt = movieRecognition.CreatedAt,
            Video = movieRecognition.Video?.ToDto(),
            Movie = movieRecognition.Movie?.ToDto(),
            Jobs = movieRecognition.Jobs.Select(JobMapper.ToDto).ToArray()
        };
    }
}