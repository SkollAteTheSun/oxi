namespace Common.RabbitMQClient.Config;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int ConsumerDispatchConcurrency { get; set; }
    public int DeadMessagesTimeoutSeconds { get; set; }
    public int DeadMessagesTryNumber { get; set; }
    public int ErrorMessagesTimeoutSeconds { get; set; }
}
