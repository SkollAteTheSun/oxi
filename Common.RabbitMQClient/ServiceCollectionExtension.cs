using Common.RabbitMQClient.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Common.RabbitMQClient;

public static class ServiceCollectionExtension
{
    public static void AddRabbitMQ(this IServiceCollection services, RabbitMQSettings settings)
    {
        services.AddSingleton<IRabbitMQApiClient, RabbitMQApiClient>(provider => new RabbitMQApiClient(
            new Uri($"http://{settings.Host}:{settings.Port}"),
            settings.Username,
            settings.Password));

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = settings.Host,
            UserName = settings.Username,
            Password = settings.Password,
            ConsumerDispatchConcurrency = settings.ConsumerDispatchConcurrency
        };
        var connection = factory.CreateConnection();

        services.AddSingleton<IRabbitMQClient, RabbitMQClient>(provider => new RabbitMQClient(
                provider.GetService<ILogger<RabbitMQClient>>(),
                connection,
                settings,
                provider.GetService<IRabbitMQApiClient>()));
    } 
}
