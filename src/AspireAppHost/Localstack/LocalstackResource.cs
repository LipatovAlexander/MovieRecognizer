namespace Aspire.AppHost.Localstack;

public class LocalstackResource(string name) : ContainerResource(name)
{
    public List<string> Services { get; } = [];
}