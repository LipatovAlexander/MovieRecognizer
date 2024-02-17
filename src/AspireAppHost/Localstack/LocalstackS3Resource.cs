namespace Aspire.AppHost.Localstack;

public class LocalstackS3Resource(LocalstackResource localstack) : Resource($"{localstack.Name}_S3"), IResourceWithParent<LocalstackResource>
{
    public LocalstackResource Parent => localstack;
}