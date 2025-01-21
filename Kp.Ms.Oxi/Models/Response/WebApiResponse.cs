namespace Kp.Ms.Oxi.Models.Response;

public class WebApiResponse<TResponse>
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public TResponse Response { get; set; }
}

