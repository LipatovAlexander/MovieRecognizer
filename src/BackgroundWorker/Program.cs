using Hangfire;
using Infrastructure;
using Infrastructure.BackgroundJobs;
using Infrastructure.DatabaseCreation;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddServiceDefaults();

builder.AddApplicationDbContext();

services.AddBackgroundJobs();
services.AddHangfireServer();

services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.EnsureDatabaseCreatedAsync("hangfire", 500);
}

app.MapDefaultEndpoints();

app.UseHangfireDashboard("");

app.Run();