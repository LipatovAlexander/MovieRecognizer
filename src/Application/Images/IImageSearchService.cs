namespace Application.Images;

public interface IImageSearchService
{
    Task<ImageSearchResponse> SearchAsync(Uri imageUrl, CancellationToken cancellationToken);
}