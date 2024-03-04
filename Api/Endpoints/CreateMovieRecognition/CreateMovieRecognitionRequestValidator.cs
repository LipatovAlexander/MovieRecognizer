using FluentValidation;

namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionRequestValidator : AbstractValidator<CreateMovieRecognitionRequest>
{
    public CreateMovieRecognitionRequestValidator()
    {
        RuleFor(x => x.VideoUrl)
            .Must(x => x.IsWellFormedOriginalString() && x.IsAbsoluteUri && (x.Scheme == Uri.UriSchemeHttps || x.Scheme == Uri.UriSchemeHttp))
            .WithMessage("Invalid url");
    }
}