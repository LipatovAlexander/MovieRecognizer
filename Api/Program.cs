using Api.Endpoints.CreateMovieRecognition;
using Api.Endpoints.GetMovieRecognition;
using Data;
using Data.YandexDb;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

services.AddData();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

var yandexDbService = app.Services.GetRequiredService<IYandexDbService>();
await yandexDbService.InitializeAsync();

app.MapDefaultEndpoints();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();