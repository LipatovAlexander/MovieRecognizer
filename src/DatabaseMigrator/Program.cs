using DatabaseCreation;
using DatabaseMigrator;
using FluentMigrator.Runner;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(configuration.GetConnectionString("application"))
        .ScanIn(typeof(Program).Assembly).For.Migrations());

services.AddHostedService<Worker>();

var host = builder.Build();

if (environment.IsDevelopment())
{
    await host.EnsureDatabaseCreatedAsync("application");
}

host.Run();