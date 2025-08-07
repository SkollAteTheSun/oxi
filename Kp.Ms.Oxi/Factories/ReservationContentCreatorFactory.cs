using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Services;

namespace Kp.Ms.Oxi.Factories;

public class ReservationContentCreatorFactory
{
    private readonly Dictionary<Type, Func<IReservationContentCreator>> _factories = new()
    {
        { typeof(Fidelio), () => new FidelioReservationContentCreatorService()}
    };
    
    public IReservationContentCreator GetCreator(Type objectType)
    {
        if (_factories.TryGetValue(objectType, out var factory))
        {
            var temp = factory();
            return temp;
        }

        throw new NullReferenceException($"Нет подходящего класса для типа {objectType.Name}");
    }
}