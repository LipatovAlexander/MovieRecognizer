using FluentValidation;

namespace Videos.API.Endpoints.GetVideo;

public sealed class GetVideoRequestValidator : AbstractValidator<GetVideoRequest>
{
    public GetVideoRequestValidator()
    {
        RuleFor(x => x.Uri)
            .Must(x => x.IsWellFormedOriginalString() && x.IsAbsoluteUri && (x.Scheme == Uri.UriSchemeHttps || x.Scheme == Uri.UriSchemeHttp))
            .WithMessage("Invalid uri");
    }
}
