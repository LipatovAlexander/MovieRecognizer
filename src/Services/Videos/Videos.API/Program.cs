using System.Text.Json.Serialization;
using VideoLibrary;
using Videos.API.Endpoints.GetVideo;
using Videos.Application;
using Videos.Application.YouTube;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton(Client.For(YouTube.Default));
builder.Services.AddSingleton<IVideoService, YouTubeVideoService>();

builder.Services.AddSingleton<IVideoFinder, VideoFinder>();

builder.Services.AddValidator<GetVideoRequest, GetVideoRequestValidator>();

var app = builder.Build();

app.UseHttpExceptionHandler();
app.UseCustomNotFoundResponseHandler();

app.MapEndpoint<GetVideoEndpoint>();

app.MapGet("/boom", () => Task.FromException<string>(new Exception("Boom!")));

app.Run();

public partial class Program;