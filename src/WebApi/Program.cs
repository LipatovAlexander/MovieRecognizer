using Infrastructure;
using Infrastructure.BackgroundJobs;
using WebApi.Endpoints.CreateMovieRecognition;
using WebApi.Endpoints.GetMovieRecognition;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

builder.AddApplicationDbContext();

services.AddBackgroundJobs();

services.AddValidator<CreateMovieRecognitionRequest, CreateMovieRecognitionRequestValidator>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapEndpoint<CreateMovieRecognitionEndpoint>();
app.MapEndpoint<GetMovieRecognitionEndpoint>();

app.Run();