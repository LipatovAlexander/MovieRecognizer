using Api.Infrastructure.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class ValidationConfiguration
{
    public static IEndpointConventionBuilder AddValidationFilter(this IEndpointConventionBuilder builder)
    {
        return builder.AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
    }
}
