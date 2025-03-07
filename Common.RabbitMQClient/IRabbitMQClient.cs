using Common.RabbitMQClient.Contracts;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace Common.RabbitMQClient;

public interface IRabbitMQClient
{
    ValueTask<List<QueueContract>> GetQueuesAsync();
    bool SendMessage<T>(string exchangeName, string queueName, string routingKey, T message, bool persistent = false);
    void CreateQueue(string exchangeName, string queueName, string routingKey);
    IModel CreateConsumer(string exchangeName, string queueName, string routingKey, Func<object?, BasicDeliverEventArgs, ValueTask> recieved);
}
