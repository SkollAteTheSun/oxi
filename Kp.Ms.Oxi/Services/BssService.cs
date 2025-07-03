using Common.HttpClientWrapper;
using Kp.Ms.Oxi.Models.Options;

namespace Kp.Ms.Oxi.Services;

public class BssService
{
    private readonly List<BssHotelOptions> _bssOptions;

    private readonly IHttpClientWrapper _httpClientWrapper;

    public BssService(IOptions<List<BssHotelOptions>> bss, IHttpClientWrapper httpClientWrapper)
    {
        _bssOptions = bss.Value;
        _httpClientWrapper = httpClientWrapper;
    }

    public async ValueTask<string?> SendUpdateRequest(long hotelId)
    {
        var option = _bssOptions.FirstOrDefault(o => o.HotelId == hotelId);

        if (option != null)
        {
            try
            {
                await _httpClientWrapper.GetAsync<string?>($"{option.Url}?{option.HotelId}");

                return $"Запрос обновления по отелю {option.HotelId} успешно выполнен";
            }
            catch (Exception ex)
            {
                return $"Запрос обновления по отелю завершен с ошибкой: {ex.Message}";
            }
        }

        return null;
    }
}