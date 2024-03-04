using Api.Endpoints.CreateMovieRecognition;
using Api.Endpoints.GetMovieRecognition;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();