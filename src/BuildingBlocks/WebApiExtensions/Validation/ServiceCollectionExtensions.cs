using FluentValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidator<TRequest, TValidator>(this IServiceCollection services) where TValidator : class, IValidator<TRequest>
    {
        return services.AddSingleton<IValidator<TRequest>, TValidator>();
    }
}
