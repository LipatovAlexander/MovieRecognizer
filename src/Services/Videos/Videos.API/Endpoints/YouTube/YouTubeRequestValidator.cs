using FluentValidation;

namespace Videos.API.Endpoints.YouTube;

public sealed class YouTubeRequestValidator : AbstractValidator<YouTubeRequest>
{
    public YouTubeRequestValidator()
    {
        RuleFor(x => x.Uri)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Invalid uri");
    }
}
