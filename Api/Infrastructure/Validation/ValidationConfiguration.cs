using Api.Infrastructure.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class ValidationConfiguration
{
    public static TBuilder AddValidationFilter<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
    }
}