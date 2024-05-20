using Api.Models;
using Domain;

namespace Api.Mappers;

public static class MovieRecognitionMapper
{
    public static MovieRecognitionDto ToDto(this MovieRecognition movieRecognition)
    {
        return new MovieRecognitionDto
        {
            Id = movieRecognition.Id,
            UserId = movieRecognition.UserId,
            VideoUrl = movieRecognition.VideoUrl,
            Status = movieRecognition.Status,
            CreatedAt = movieRecognition.CreatedAt,
            RecognizedMovie = movieRecognition.RecognizedMovie?.ToDto(),
            FailureMessage = movieRecognition.FailureMessage
        };
    }
}