using Aspire.AppHost.Localstack;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.Configuration["PostgresPassword"];

if (builder.Environment.IsDevelopment() && string.IsNullOrEmpty(postgresPassword))
{
    throw new InvalidOperationException("""
                                        A password for the local PostgreSQL Server container is not configured.
                                        Add one to the AppHost project's user secrets with the key 'PostgresPassword', e.g. dotnet user-secrets set PostgresPassword <password>
                                        """);
}

var postgres = builder
    .AddPostgresContainer("postgres", password: postgresPassword)
    .WithPgAdmin()
    .WithVolumeMount("VolumeMount.postgres.data", "/var/lib/postgresql/data", VolumeMountType.Named);

var hangfireDatabase = postgres.AddDatabase("hangfire");
var applicationDatabase = postgres.AddDatabase("application");

var localstack = builder.AddLocalstack("localstack")
    .AddS3();

builder.AddProject<Projects.DatabaseMigrator>("database-migrator")
    .WithReference(postgres)
    .WithReference(applicationDatabase);

builder.AddProject<Projects.WebApi>("web-api")
    .WithReference(hangfireDatabase)
    .WithReference(applicationDatabase);

builder.AddProject<Projects.BackgroundWorker>("background-worker")
    .WithReference(postgres)
    .WithReference(applicationDatabase)
    .WithReference(hangfireDatabase)
    .WithReference(localstack);

builder.Build().Run();