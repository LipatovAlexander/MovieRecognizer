using Aspire.AppHost.Localstack;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgresContainer("postgres")
    .WithPgAdmin();

var hangfireDatabase = postgres.AddDatabase("hangfire");
var applicationDatabase = postgres.AddDatabase("application");

var localstack = builder.AddLocalstack("localstack");

builder.AddProject<Projects.DatabaseMigrator>("database-migrator")
    .WithReference(postgres)
    .WithReference(applicationDatabase);

builder.AddProject<Projects.WebApi>("web-api")
    .WithReference(postgres)
    .WithReference(hangfireDatabase)
    .WithReference(applicationDatabase)
    .WithReference(localstack);

builder.Build().Run();