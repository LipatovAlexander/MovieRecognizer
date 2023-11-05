using VideoLibrary;
using Videos.API.Endpoints.YouTube;
using Videos.Application.YouTube;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(Client.For(YouTube.Default));
builder.Services.AddSingleton<IYouTubeVideoService, YouTubeVideoService>();

builder.Services.AddValidator<YouTubeRequest, YouTubeRequestValidator>();

var app = builder.Build();

app.UseHttpExceptionHandler();
app.UseCustomNotFoundResponseHandler();

app.MapEndpoint<YouTubeEndpoint>();

app.Run();
