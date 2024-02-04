using System.Text.Json.Serialization;
using Application;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>("application");
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("recognize", async (Uri videoUrl, IApplicationDbContext dbContext) =>
{
    var request = new RecognitionRequest(videoUrl, DateTimeOffset.UtcNow, RecognitionRequestStatus.Created);
    dbContext.RecognitionRequests.Add(request);
    await dbContext.SaveChangesAsync();

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