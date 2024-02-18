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
                context.EnvironmentVariables.Add("SERVICES", "s3");
                context.EnvironmentVariables.Add("AWS_DEFAULT_REGION", "us-east-1");
                context.EnvironmentVariables.Add("AWS_ACCESS_KEY_ID", "user");
                context.EnvironmentVariables.Add("AWS_SECRET_ACCESS_KEY", "password");
            })
            .WithVolumeMount("VolumeMount.localstack.volume", "/var/lib/localstack", VolumeMountType.Named)
            .WithVolumeMount("VolumeMount.localstack.docker_sock", "/var/run/docker.sock", VolumeMountType.Named)
            .WithVolumeMount("./Localstack/create-resources.sh", "/etc/localstack/init/ready.d/create-resources.sh")
            .ExcludeFromManifest();
    }

    public static IResourceBuilder<TDestination> WithReference<TDestination>(
        this IResourceBuilder<TDestination> builder,
        IResourceBuilder<LocalstackResource> source)
        where TDestination : IResourceWithEnvironment
    {
        var localstack = source.Resource;
        
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

                context.EnvironmentVariables.TryAdd("FileStorage__BucketName", "application");
            });
    }
}