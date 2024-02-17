namespace Aspire.AppHost.Localstack;

public static class LocalstackBuilderExtensions
{
    private const int DefaultContainerPort = 4566;

    public static IResourceBuilder<LocalstackResource> AddLocalstack(
        this IDistributedApplicationBuilder builder,
        string name)
    {
        var localstack = new LocalstackResource(name);

        return builder
            .AddResource(localstack)
            .WithEndpoint(DefaultContainerPort)
            .WithAnnotation(new ContainerImageAnnotation { Image = "localstack/localstack", Tag = "latest" })
            .WithEnvironment(context =>
            {
                context.EnvironmentVariables.Add("SERVICES", string.Join(',', localstack.Services));
                context.EnvironmentVariables.Add("AWS_DEFAULT_REGION", "us-east-1");
                context.EnvironmentVariables.Add("AWS_ACCESS_KEY_ID", "user");
                context.EnvironmentVariables.Add("AWS_SECRET_ACCESS_KEY", "password");
            })
            .WithVolumeMount("VolumeMount.localstack.data", "/var/lib/localstack", VolumeMountType.Named)
            .WithVolumeMount("VolumeMount.docker_sock.data", "/var/run/docker.sock", VolumeMountType.Named)
            .ExcludeFromManifest();
    }

    public static IResourceBuilder<LocalstackS3Resource> AddS3(this IResourceBuilder<LocalstackResource> builder)
    {
        var localstack = builder.Resource;
        var localstackS3 = new LocalstackS3Resource(localstack);
        
        localstack.Services.Add("s3");

        return builder.ApplicationBuilder
            .AddResource(localstackS3)
            .ExcludeFromManifest();
    }

    public static IResourceBuilder<TDestination> WithReference<TDestination, TLocalstackServiceResource>(
        this IResourceBuilder<TDestination> builder,
        IResourceBuilder<TLocalstackServiceResource> source)
        where TDestination : IResourceWithEnvironment
        where TLocalstackServiceResource : IResourceWithParent<LocalstackResource>
    {
        var localstack = source.Resource.Parent;
        
        return builder
            .WithEnvironment(context =>
            {
                if (context.PublisherName == "manifest")
                {
                    return;
                }
                
                if (!localstack.TryGetAllocatedEndPoints(out var allocatedEndpoints))
                {
                    throw new DistributedApplicationException("Expected allocated endpoints!");
                }

                var allocatedEndpoint = allocatedEndpoints.Single();
                
                context.EnvironmentVariables.TryAdd("AWS_SERVICE_URL", $"http://{allocatedEndpoint.Address}:{allocatedEndpoint.Port}");
                context.EnvironmentVariables.TryAdd("AWS_DEFAULT_REGION", "us-east-1");
                context.EnvironmentVariables.TryAdd("AWS_ACCESS_KEY_ID", "user");
                context.EnvironmentVariables.TryAdd("AWS_SECRET_ACCESS_KEY", "password");
            });
    }
}