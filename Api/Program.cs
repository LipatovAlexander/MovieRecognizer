using Api.Endpoints.CreateMovieRecognition;
using Api.Endpoints.GetMovieRecognition;
using Api.Infrastructure.Authentication;
using Data;
using Files;
using MessageQueue;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = AuthConstants.ApiKeyHeaderName,
        Type = SecuritySchemeType.ApiKey
    });
});

services.AddData();
services.AddMessageQueue();
services.AddFileStorage();

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