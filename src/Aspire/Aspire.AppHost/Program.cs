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
    .WithVolumeMount("VolumeMount.postgres.data", "/var/lib/postgresql/data", VolumeMountType.Named);

var hangfireDatabase = postgres.AddDatabase("hangfire");

builder.AddProject<Projects.BackgroundWorker_Host>("background-worker")
    .WithReference(postgres)
    .WithReference(hangfireDatabase);

builder.Build().Run();