namespace Common.RabbitMQClient.Exceptions;

public class RabbitMQClientException : Exception
{
    public RabbitMQClientException() : base()
    {

    }

    public RabbitMQClientException(string message) : base(message)
    {

    }

    public RabbitMQClientException(string message, Exception ex) : base(message, ex)
    {

    }
}
