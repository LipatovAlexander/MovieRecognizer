using Hangfire;
using Infrastructure;
using Infrastructure.BackgroundJobs;
using Infrastructure.DatabaseCreation;
using WebApi.Endpoints.CreateMovieRecognition;
using WebApi.Endpoints.GetMovieRecognition;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

builder.AddApplicationDbContext();

services.AddBackgroundJobs();
services.AddHangfireServer();

services.AddAmazonS3Client();
services.AddApplicationServices();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.EnsureDatabaseCreatedAsync("hangfire", 500);
}

app.MapDefaultEndpoints();

app.UseHangfireDashboard();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();