using Kp.Ms.Oxi.Models.Options;
using Microsoft.Extensions.Options; 

namespace Kp.Ms.Oxi.Services;

public class BssService
{
    private readonly List<BssHotelOptions> _bssOptions;

    private readonly HttpClient _httpClient;

    public BssService(IOptions<List<BssHotelOptions>> bss)
    {
        _bssOptions = bss.Value;
        _httpClient = new HttpClient();
    }

    public async ValueTask<string?> SendUpdateRequest(long hotelId)
    {
        var option = _bssOptions.FirstOrDefault(o => o.HotelId == hotelId);

        if (option != null)
        {
            try
            {
                var responseMessage = await _httpClient.DeleteAsync(new Uri($"{option.Url}?{option.HotelId}"));

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return "В bss сервисе обновление прошло успешно!";
                }
                else
                {
                    var response = await responseMessage.Content.ReadAsStringAsync();

                    return $"Обновление в bss завершилось ошибкой: {response}";
                }
            }
            catch (Exception ex)
            {
                return $"Возникла ошибка при отправке запроса в bss: {ex.Message}";
            }
        }

        return null;
    }
}