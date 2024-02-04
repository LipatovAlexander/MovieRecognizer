using BackgroundWorker.Host;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.AddServiceDefaults();
builder.AddNpgsqlDataSource("postgres");

services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"));
    });
});

services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.EnsureDatabaseCreatedAsync();
}

app.MapDefaultEndpoints();

app.UseHangfireDashboard();

app.Run();