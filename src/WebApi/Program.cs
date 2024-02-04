using System.Text.Json.Serialization;
using Application;
using Domain;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("recognize", async (Uri videoUrl, IApplicationDbContext dbContext, IBackgroundJobClient backgroundJob) =>
{
    var request = new RecognitionRequest(videoUrl, DateTimeOffset.UtcNow, RecognitionRequestStatus.Created);
    dbContext.RecognitionRequests.Add(request);
    await dbContext.SaveChangesAsync();

    backgroundJob.Enqueue(() => Console.WriteLine($"Request {request.Id} created"));

    return Results.Created("recognize", new { id = request.Id });
});

app.MapGet("recognize/{id:guid}", async (Guid id, IApplicationDbContext dbContext) =>
{
    var request = await dbContext.RecognitionRequests.FirstOrDefaultAsync(r => r.Id == id);

    return request is null
        ? Results.NotFound()
        : Results.Ok(request);
});

app.Run();