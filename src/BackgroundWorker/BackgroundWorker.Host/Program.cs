using Application;
using DatabaseCreation;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>("application");
services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"));
    });
});

services.AddHangfireServer();

services.AddApplicationCommands();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.EnsureDatabaseCreatedAsync("hangfire", 500);
}

app.MapDefaultEndpoints();

app.UseHangfireDashboard();

app.Run();