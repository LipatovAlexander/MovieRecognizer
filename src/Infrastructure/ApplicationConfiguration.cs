using Application;
using Application.Commands.StartMovieRecognition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class ApplicationConfiguration
{
    public static void AddApplicationDbContext(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("application");
        builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
    
    public static void AddApplicationCommands(this IServiceCollection services)
    {
        services.AddScoped<IStartMovieRecognitionCommandHandler, StartMovieRecognitionCommandHandler>();
    }
}