var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgresContainer("postgres", password: "postgres_password")
    .WithVolumeMount("VolumeMount.postgres.data", "/var/lib/postgresql/data", VolumeMountType.Named)
    .WithPgAdmin();

var hangfireDatabase = postgres.AddDatabase("hangfire");
var applicationDatabase = postgres.AddDatabase("application");

builder.AddProject<Projects.DatabaseMigrator>("database-migrator")
    .WithReference(postgres)
    .WithReference(applicationDatabase);

builder.AddProject<Projects.WebApi>("web-api")
    .WithReference(postgres)
    .WithReference(hangfireDatabase)
    .WithReference(applicationDatabase);

builder.Build().Run();