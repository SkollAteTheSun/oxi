using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Interfaces;

public interface IReservationContentCreator
{
    // возвращаю json, чтобы потом скастить его к нужному объекту
    string GetReservationContent(SendReservationRequest request, string decryptedPassword);
}