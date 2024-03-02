using System.Net.Http.Json;

namespace Application.SerpApi;

public interface IYandexReverseImageApi
{
    Task<YandexReverseImageSearchResponse> SearchAsync(Uri imageUrl, CancellationToken cancellationToken);
}

public class YandexReverseImageApi(SerpApiSettings settings, HttpClient httpClient) : IYandexReverseImageApi
{
    private readonly SerpApiSettings _settings = settings;
    private readonly HttpClient _httpClient = httpClient;
    
    public async Task<YandexReverseImageSearchResponse> SearchAsync(Uri imageUrl, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<YandexReverseImageSearchResponse>($"/search?engine=yandex_images&url={imageUrl.ToString()}&api_key={_settings.ApiKey}", cancellationToken);
        
        return response!;
    }
}