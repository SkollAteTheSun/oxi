using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Common;
using Kp.Ms.Oxi.Factories;
using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Services;

public class XmlCreatorService : IXmlCreator
{
    private readonly IConfiguration _config;
    private readonly XmlWriterSettings _xmlSettings;
    private readonly ReservationContentCreatorFactory _reservationContentFactory;
    
    public XmlCreatorService(IConfiguration config, ReservationContentCreatorFactory reservationContentFactory)
    {
        _config = config;
        _xmlSettings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };
        _reservationContentFactory = reservationContentFactory;
    }
    
    public string CreateXml(SendReservationRequest request, Type type)
    {
        MakeTransactionId(request);

        var decryptedPassword = GetDecryptedPassword(request);

        var creator = _reservationContentFactory.GetCreator(type);
        var reservationContent = creator.GetReservationContent(request, decryptedPassword);
        
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

                string xmlHeader = GetXmlHeader(request, type);

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                xml = sb.ToString();
            }
        }

        return xml;
    }

    private string GetXmlHeader(SendReservationRequest request, Type type)
    {
        return type.Name switch
        {
            nameof(Fidelio) => $"""
                                        <?xml version="1.0" encoding="UTF-8"?>
                                        """,
            nameof(Reservation) => $"""
                                            <?xml version = '1.0'?>
                                            <?Label {request.Interface.HotelCode}|RESERVATION|{request.Interface.TransactionId}|{request.Interface.Status}?>
                                            """,
        };
    }

    private string GetDecryptedPassword(SendReservationRequest request)
    {
        byte[] bytes = Convert.FromBase64String(request.Interface.Auth.Password);
        byte[] key = Encoding.ASCII.GetBytes(_config["AesEncryption:Key"] ?? throw new Exception("Aes key is not set"));
        byte[] iv = Encoding.ASCII.GetBytes(request.Interface.Auth.Iv);
        
        return AesEncryption.Decrypt(bytes, key, iv);
    }

    private void MakeTransactionId(SendReservationRequest request)
    {
        request.Interface.TransactionId = "BK" + request.Interface.TransactionId;
    }
}