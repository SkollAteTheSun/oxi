using Common.RabbitMQClient.Contracts;

namespace Common.RabbitMQClient;

public interface IRabbitMQApiClient
{
    ValueTask<List<QueueContract>> GetQueuesAsync();
}
