using Common.RabbitMQClient.Config;
using Common.RabbitMQClient.Contracts;
using Common.RabbitMQClient.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common.RabbitMQClient;

public class RabbitMQClient : IRabbitMQClient, IDisposable
{
    private readonly IConnection _connection;
    private readonly RabbitMQSettings _settings;
    private readonly IRabbitMQApiClient _api;
    private readonly ILogger<RabbitMQClient> _logger;
    private readonly IModel _channel;

    private string _deadMessagesPrefix;
    private string _errorMessagesPrefix;

    public RabbitMQClient(
        ILogger<RabbitMQClient> logger,
        IConnection connection,
        RabbitMQSettings settings,
        IRabbitMQApiClient api)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _channel = _connection.CreateModel();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public async ValueTask<List<QueueContract>> GetQueuesAsync()
    {
        try
        {
            return await _api.GetQueuesAsync();
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error getting queue list: {ex.Message}";
            _logger.LogError(errorMessage);
            throw new RabbitMQClientException(errorMessage);
        }
    }

    public bool SendMessage<T>(string exchangeName, string queueName, string routingKey, T message)
    {
        try
        {
            CreateQueue(exchangeName, queueName, routingKey);
            var messageJsonString = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(messageJsonString);
            _channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

            _logger.LogDebug($"A new task has been added to the queue \"{queueName}\": {messageJsonString}");
            return true;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error adding task to queue \"{queueName}\": {ex.Message}";
            _logger.LogError(errorMessage);
            throw new RabbitMQClientException(errorMessage);
        }
    }

    public void CreateQueue(string exchangeName, string queueName, string routingKey)
    {
        try
        {
            var arguments = new Dictionary<string, object>()
            {
                { "x-single-active-consumer", true }
            };

            if (_settings.DeadMessagesTimeoutSeconds > 0) {
                _deadMessagesPrefix = CreateDeadMessagesQueue(exchangeName, queueName, routingKey, _settings.DeadMessagesTimeoutSeconds);
                arguments.Add("x-dead-letter-exchange", $"{_deadMessagesPrefix}{exchangeName}");
            }

            if (_settings.ErrorMessagesTimeoutSeconds > 0)
            {
                _errorMessagesPrefix = CreateDeadMessagesQueue(exchangeName, queueName, routingKey, _settings.ErrorMessagesTimeoutSeconds);
            }

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
            _channel.QueueDeclare(queueName, true, false, false, arguments);
            _channel.QueueBind(queueName, exchangeName, routingKey, null);

            _logger.LogInformation($"The \"{queueName}\" queue has been created.");
        }
        catch (Exception ex)
        {
            var errorMessage = $"Failed to create queue: {ex.Message}";
            _logger.LogError(errorMessage);
            throw new RabbitMQClientException(errorMessage);
        }
    }

    public IModel CreateConsumer(string exchangeName, string queueName, string routingKey, Func<object?, BasicDeliverEventArgs, ValueTask> recieved)
    {
        try
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, arg) =>
            {
                try
                {
                    try
                    {
                        await recieved.Invoke(model, arg);
                        _channel.BasicAck(arg.DeliveryTag, false);
                    }
                    catch (Exception e)
                    {
                        if (_settings.DeadMessagesTryNumber == 0 || Convert.ToInt32(arg.DeliveryTag) <= _settings.DeadMessagesTryNumber)
                        {
                            _channel.BasicReject(arg.DeliveryTag, false);
                            _logger.LogError($"Failed to process message from queue: {e.Message}");
                        }
                        else
                        {
                            _logger.LogError($"The number of attempts to complete the task was exceeded due to an error: {e.Message}");
                            _channel.BasicAck(arg.DeliveryTag, false);

                            _channel.BasicPublish($"{_errorMessagesPrefix}{exchangeName}", routingKey, null, arg.Body.ToArray());
                            _logger.LogError($"The task has been sent to the waiting queue for {_settings.ErrorMessagesTimeoutSeconds} seconds.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $"An unexpected error occurred while processing a message from the queue: {ex.Message}";
                    _logger.LogError(errorMessage);
                    throw new RabbitMQClientException(errorMessage);
                }
            };

            _channel.BasicQos(0, 1, false);
            _channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            _logger.LogInformation($"Handler for queue \"{queueName}\" created.");

            return _channel;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An unexpected error occurred while creating the message handler: {ex.Message}";
            _logger.LogError(errorMessage);
            throw new RabbitMQClientException(errorMessage);
        }
    }

    private string CreateDeadMessagesQueue(string exchangeName, string queueName, string routingKey, int timeoutSeconds)
    {
        var deadMessagesPrefix = $"dead-{timeoutSeconds}s-";
        var deadMessagesQueueName = $"{deadMessagesPrefix}{queueName}";
        var deadMessagesExchangeName = $"{deadMessagesPrefix}{exchangeName}";
        var deadMessagesRoutingKey = $"{deadMessagesPrefix}{routingKey}";

        _channel.ExchangeDeclare(deadMessagesExchangeName, ExchangeType.Fanout, true);
        _channel.QueueDeclare(
            deadMessagesQueueName,
            true,
            false,
            false,
            arguments: new Dictionary<string, object>()
            {
                    { "x-message-ttl", timeoutSeconds * 1000 },
                    { "x-dead-letter-exchange", exchangeName }
            });
        _channel.QueueBind(deadMessagesQueueName, deadMessagesExchangeName, deadMessagesRoutingKey, null);

        return deadMessagesPrefix;
    }
}
