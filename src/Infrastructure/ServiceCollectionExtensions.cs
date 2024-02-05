using Application.Commands.StartMovieRecognition;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCommands(this IServiceCollection services)
    {
        services.AddScoped<IStartMovieRecognitionCommandHandler, StartMovieRecognitionCommandHandler>();
    }
}