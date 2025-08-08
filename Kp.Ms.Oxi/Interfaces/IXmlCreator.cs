using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Interfaces;

public interface IXmlCreator
{
    string CreateXml(SendReservationRequest request, Type type);
}