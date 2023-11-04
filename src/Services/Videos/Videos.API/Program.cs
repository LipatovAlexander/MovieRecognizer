using Videos.Application.YouTube;
using VideoLibrary;
using WebApiExtensions.Middlewares;
using WebApiExtensions.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(Client.For(YouTube.Default));
builder.Services.AddSingleton<IYouTubeVideoService, YouTubeVideoService>();

builder.Services.AddEndpoints<Program>();
builder.Services.AddValidators<Program>();

var app = builder.Build();

app.UseHttpExceptionHandler();
app.UseCustomNotFoundResponseHandler();

app.MapEndpoints()
    .AddValidationFilter();

app.Run();
