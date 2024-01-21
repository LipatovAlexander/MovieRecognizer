using System.Text.Json.Serialization;
using Recognizer.API.Endpoints.RecognizeMovie;
using Recognizer.Application;
using Recognizer.Application.FramesSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IRecognitionStrategy, FramesSearchRecognitionStrategy>();
builder.Services.AddSingleton<IRecognitionService, RecognitionService>();

builder.Services.AddValidator<RecognizeMovieRequest, RecognizeMovieRequestValidator>();

var app = builder.Build();

app.UseHttpExceptionHandler();
app.UseCustomNotFoundResponseHandler();

app.MapEndpoint<RecognizeMovieEndpoint>();

app.MapGet("/boom", () => Task.FromException<string>(new Exception("Boom!")));

app.Run();

public partial class Program;