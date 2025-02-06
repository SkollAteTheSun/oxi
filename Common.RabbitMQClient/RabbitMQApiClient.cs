using Common.RabbitMQClient.Contracts;
using System.Text;
using System.Text.Json;

namespace Common.RabbitMQClient;

public class RabbitMQApiClient : IRabbitMQApiClient
{
    private readonly HttpClient _httpClient;

    public RabbitMQApiClient(
        Uri baseUri,
        string username,
        string password)
    {
        _baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _password = password ?? throw new ArgumentNullException(nameof(password));
        
        var socketsHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        _httpClient = new HttpClient(socketsHandler);
        var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
            .GetBytes(_username + ":" + _password));
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
    }

    public async ValueTask<List<QueueContract>> GetQueuesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUri}api/queues");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        var stringResult = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<QueueContract>>(stringResult) ?? new List<QueueContract>();
    }

    private readonly Uri _baseUri;
    private readonly string _username;
    private readonly string _password;
}
