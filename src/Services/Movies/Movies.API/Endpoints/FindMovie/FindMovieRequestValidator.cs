using FluentValidation;

namespace Movies.API.Endpoints.FindMovie;

public sealed class FindMovieRequestValidator : AbstractValidator<FindMovieRequest>
{
    public FindMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}
