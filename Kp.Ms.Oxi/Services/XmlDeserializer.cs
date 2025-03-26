using System.Xml.Serialization;
using System.Xml;

namespace Kp.Ms.Oxi.Services;

public class XmlDeserializer
{
    private readonly LogService _logService;

    public XmlDeserializer(LogService logService)
    {
        _logService = logService;
    }

    public TModel DeserializeXmlToMessage<TModel>(XmlDocument xmlDocument) where TModel : class
    {
        try
        {
            var rootNode = xmlDocument.DocumentElement;
            if (rootNode == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TModel));
            using var reader = new StringReader(rootNode.OuterXml);
            using var xmlReader = XmlReader.Create(reader);

            return (TModel?)serializer.Deserialize(xmlReader);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"XML deserialization error: {ex.Message}");
            return null;
        }
    }
}