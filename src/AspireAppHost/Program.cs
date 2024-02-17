using Aspire.AppHost.Localstack;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgresContainer("postgres")
    .WithPgAdmin();

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