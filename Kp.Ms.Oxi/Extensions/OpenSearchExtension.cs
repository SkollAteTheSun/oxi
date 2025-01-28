using Kp.Ms.Oxi.Models.Options;
using Newtonsoft.Json.Serialization;
using OpenSearch.Client;
using OpenSearch.Client.JsonNetSerializer;
using OpenSearch.Net;

namespace Kp.Ms.Oxi.Extensions;

public static class OpenSearchExtension
{
    public static void AddOpenSearch(this IServiceCollection services, IConfiguration configuration)
    {
        OpenSearchOptions openSearchOptions =
            configuration.GetSection(OpenSearchOptions.Position).Get<OpenSearchOptions>() ??
            throw new InvalidOperationException("Open Search options is not configured");

        var pool = new SingleNodeConnectionPool(new Uri(openSearchOptions.Url));
        var connectionSettings = new ConnectionSettings(pool, (builtin, settings) =>
                new JsonNetSerializer(builtin, settings,
                    modifyContractResolver: c => c.NamingStrategy = new SnakeCaseNamingStrategy()
                )
            )
            .ServerCertificateValidationCallback((_, _, _, _) => true)
            .BasicAuthentication(openSearchOptions.User, openSearchOptions.Password);
        var client = new OpenSearchClient(connectionSettings);

        services.AddSingleton(client);
    }
}