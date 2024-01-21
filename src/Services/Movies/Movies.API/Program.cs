using System.Text.Json.Serialization;
using Movies.API.Endpoints.FindMovie;
using Movies.Application;
using Movies.Application.OMDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOptions<OMDbSettings>()
    .BindConfiguration(OMDbSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<IMovieService, OMDbMovieService>();

builder.Services.AddValidator<FindMovieRequest, FindMovieRequestValidator>();

var app = builder.Build();

app.UseHttpExceptionHandler();
app.UseCustomNotFoundResponseHandler();

app.MapEndpoint<FindMovieEndpoint>();

if (!builder.Environment.IsProduction())
{
    app.MapGet("/boom", () => Task.FromException<string>(new Exception("Boom!")));
}

app.Run();

public partial class Program;