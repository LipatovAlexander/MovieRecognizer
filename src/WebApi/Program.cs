using System.Text.Json.Serialization;
using Application;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;
using WebApi.Endpoints.CreateMovieRecognition;
using WebApi.Endpoints.GetMovieRecognition;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>("application");
services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"));
    });
});

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCustomNotFoundResponseHandler();
app.UseHttpExceptionHandler();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();