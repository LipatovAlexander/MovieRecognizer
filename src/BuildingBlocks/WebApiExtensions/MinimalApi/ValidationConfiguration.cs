using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiExtensions.MinimalApi;

public static class ValidationConfiguration
{
    public static IServiceCollection AddValidators<TAssemblyMarker>(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        return services.AddValidatorsFromAssemblyContaining<TAssemblyMarker>(ServiceLifetime.Singleton);
    }

    public static IEndpointConventionBuilder AddValidationFilter(this IEndpointConventionBuilder builder)
    {
        return builder.AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
    }
}
