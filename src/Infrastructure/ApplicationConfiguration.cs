using Application;
using Application.Commands;
using Application.Commands.ExtractFrames;
using Application.Commands.FinishMovieRecognition;
using Application.Commands.RecognizeMovie;
using Application.Commands.ScrapeVideoInformation;
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
        services.AddScoped<ICommandHandler<StartMovieRecognitionCommand>, StartMovieRecognitionCommandHandler>();
        services.AddScoped<ICommandHandler<ScrapeVideoInformationCommand>, ScrapeVideoInformationCommandHandler>();
        services.AddScoped<ICommandHandler<ExtractFramesCommand>, ExtractFramesCommandHandler>();
        services.AddScoped<ICommandHandler<RecognizeMovieCommand>, RecognizeMovieCommandHandler>();
        services.AddScoped<ICommandHandler<FinishMovieRecognitionCommand>, FinishMovieRecognitionCommandHandler>();
    }
}