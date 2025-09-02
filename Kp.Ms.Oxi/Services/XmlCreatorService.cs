using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Factories;
using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Services;

public class XmlCreatorService : IXmlCreator
{
    private readonly XmlWriterSettings _xmlSettings;
    private readonly ReservationContentCreatorFactory _reservationContentFactory;
    
    public XmlCreatorService(IConfiguration config, ReservationContentCreatorFactory reservationContentFactory)
    {
        _xmlSettings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };
        _reservationContentFactory = reservationContentFactory;
    }
    
    public string CreateXml(SendReservationRequest request, Type type)
    {
        MakeTransactionId(request);
        MakeComment(request);

        var creator = _reservationContentFactory.GetCreator(type);
        var reservationContent = creator.GetReservationContent(request);
        var xmlHeader = creator.GetXmlHeader(request);
        
        // 🔧 Сериализация XML
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);
        XmlSerializer xmlSerializer = new XmlSerializer(type);
        string xmlBody = "";

        string xml;
        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww, _xmlSettings))
            {
                xmlSerializer.Serialize(writer, reservationContent, ns);
                xmlBody = sww.ToString();

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                xml = sb.ToString();
            }
        }

        return xml;
    }

    private void MakeTransactionId(SendReservationRequest request)
    {
        request.Interface.TransactionId = "BK" + request.Interface.TransactionId;
    }
    
    private void MakeComment(SendReservationRequest request)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("tot: ");
        sb.Append(request.Reservation.RoomStay.Amount.ToString("0.00"));
        sb.Append('\n');
        sb.Append("paid: ");
        sb.Append(request.Reservation.PaymentAmount.ToString("0.00"));

        if (request.Reservation.ChildrenAges.Count > 0)
        {
            sb.Append('\n');
            sb.AppendJoin(" / ", request.Reservation.ChildrenAges.Select(childrenAge => $"реб {childrenAge} лет").ToList());
        }

        if (!string.IsNullOrEmpty(request.Reservation.Comment))
        {
            sb.Append('\n');
            sb.Append(request.Reservation.Comment);
        }

        request.Reservation.Comment = sb.ToString();
    }
}