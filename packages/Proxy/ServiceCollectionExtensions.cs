using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Proxy;

public static class ServiceCollectionExtensions
{
    public static void AddProxyHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("proxy")
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var proxyAddress = configuration["PROXY_ADDRESS"]
                                   ?? throw new InvalidOperationException("PROXY_ADDRESS configuration required");

                var proxyLogin = configuration["PROXY_LOGIN"]
                                 ?? throw new InvalidOperationException("PROXY_LOGIN configuration required");

                var proxyPassword = configuration["PROXY_PASSWORD"]
                                    ?? throw new InvalidOperationException("PROXY_PASSWORD configuration required");

                return new HttpClientHandler
                {
                    Proxy = new WebProxy
                    {
                        Address = new Uri(proxyAddress),
                        Credentials = new NetworkCredential(proxyLogin, proxyPassword)
                    }
                };
            });
    }
}