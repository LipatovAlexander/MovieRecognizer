using Api.Endpoints.CreateMovieRecognition;
using Api.Endpoints.GetMovieRecognition;
using Data;
using MessageQueue;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddData();
services.AddMessageQueue();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

var yandexDbService = app.Services.GetRequiredService<IYandexDbService>();
await yandexDbService.InitializeAsync();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();