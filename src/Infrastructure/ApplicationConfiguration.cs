using Application;
using Application.Videos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoLibrary;

namespace Infrastructure;

public static class ApplicationConfiguration
{
    public static void AddApplicationDbContext(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("application",
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    dbContextOptionsBuilder.EnableSensitiveDataLogging();
                }
            });
    
        builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(Client.For(YouTube.Default));
        services.AddSingleton<IVideoService, VideoService>();
    }
}