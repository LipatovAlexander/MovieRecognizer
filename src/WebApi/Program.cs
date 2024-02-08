using System.Text.Json.Serialization;
using Infrastructure.BackgroundJobs;
using Infrastructure.Configurations;
using WebApi.Endpoints.CreateMovieRecognition;
using WebApi.Endpoints.GetMovieRecognition;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

builder.AddApplicationDbContext();

services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddBackgroundJobs();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCustomNotFoundResponseHandler();
app.UseHttpExceptionHandler();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();