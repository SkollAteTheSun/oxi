using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Services;

namespace Kp.Ms.Oxi.Factories;

public class ReservationContentCreatorFactory
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<Type, Func<IReservationContentCreator>> _factories;

    public ReservationContentCreatorFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _factories = new Dictionary<Type, Func<IReservationContentCreator>>
        {
            { typeof(Fidelio), () => new FidelioReservationContentCreatorService(_configuration) }
        };
    }
    
    public IReservationContentCreator GetCreator(Type objectType)
    {
        if (_factories.TryGetValue(objectType, out var creator))
        {
            return creator();
        }
        
        throw new NullReferenceException($"Нет подходящего класса для типа {objectType.Name}");
    }
}