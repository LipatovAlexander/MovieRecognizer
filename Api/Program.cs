using Api.Endpoints.ConfirmRecognitionCorrectness;
using Api.Endpoints.CreateMovieRecognition;
using Api.Endpoints.GetMovieRecognition;
using Api.Endpoints.GetMovieRecognitionHistory;
using Api.Endpoints.GetMovieRecognitionStatistics;
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

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        [scheme] = []
    };
    x.AddSecurityRequirement(requirement);
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
app.MapEndpoint<GetMovieRecognitionHistoryEndpoint>();
app.MapEndpoint<ConfirmRecognitionCorrectnessEndpoint>();
app.MapEndpoint<GetMovieRecognitionStatisticsEndpoint>();

app.Run();