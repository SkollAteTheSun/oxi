using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Interfaces;

public interface IReservationContentCreator
{
    IReservationContent GetReservationContent(SendReservationRequest request);
    
    string GetXmlHeader(SendReservationRequest request);
}