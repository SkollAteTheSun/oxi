using Autofac;
using Common.RabbitMQClient.Config;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common.RabbitMQClient;

public class AutofacModule : Module
{
    public AutofacModule(RabbitMQSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new RabbitMQApiClient(
                new Uri($"http://{_settings.Host}:{_settings.Port}"),
                _settings.Username,
                _settings.Password))
            .As<IRabbitMQApiClient>()
            .SingleInstance();

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password,
            ConsumerDispatchConcurrency = _settings.ConsumerDispatchConcurrency
        };
        var connection = factory.CreateConnection();

        builder.Register(ctx => new RabbitMQClient(
                ctx.Resolve<ILogger<RabbitMQClient>>(),
                connection,
                _settings,
                ctx.Resolve<IRabbitMQApiClient>()))
            .As<IRabbitMQClient>()
            .SingleInstance();
    }

    private readonly RabbitMQSettings _settings;
}