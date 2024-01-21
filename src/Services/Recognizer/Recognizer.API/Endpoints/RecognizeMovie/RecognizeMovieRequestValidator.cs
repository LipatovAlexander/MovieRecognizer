using FluentValidation;

namespace Recognizer.API.Endpoints.RecognizeMovie;

public sealed class RecognizeMovieRequestValidator : AbstractValidator<RecognizeMovieRequest>
{
    public RecognizeMovieRequestValidator()
    {
        RuleFor(x => x.VideoUri)
            .Must(x => x.IsWellFormedOriginalString() && x.IsAbsoluteUri && (x.Scheme == Uri.UriSchemeHttps || x.Scheme == Uri.UriSchemeHttp))
            .WithMessage("Invalid uri");
    }
}
