using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Package
{
    [XmlAttribute("calculationRule")]
    public string CalculationRule { get; set; }

    [XmlAttribute("postingRhythm")]
    public string PostingRhythm { get; set; }

    [XmlAttribute("packageElement")]
    public string PackageElement { get; set; }

    [XmlAttribute("addToRateMode")]
    public string AddToRateMode { get; set; }

    [XmlElement("hotelCode")]
    public string HotelCode { get; set; }

    [XmlElement("packageCode")]
    public string PackageCode { get; set; }

    [XmlElement("packageDescription")]
    public string PackageDescription { get; set; }

    [XmlElement("printSeperately")]
    public int PrintSeparately { get; set; }

    [XmlElement("addToRate")]
    public int AddToRate { get; set; }

    [XmlElement("taxIncluded")]
    public int TaxIncluded { get; set; }

    [XmlElement("allowance")]
    public int Allowance { get; set; }

    [XmlElement("currencyCode")]
    public string CurrencyCode { get; set; }

    [XmlElement("sellSeparate")]
    public int SellSeparate { get; set; }

    [XmlElement("postNextDay")]
    public int PostNextDay { get; set; }

    [XmlElement("cateringPackage")]
    public int CateringPackage { get; set; }

    [XmlElement("forecastNextDay")]
    public int ForecastNextDay { get; set; }

    [XmlElement("overrideFixedRate")]
    public int OverrideFixedRate { get; set; }

    [XmlElement("containsInactiveDetails")]
    public int ContainsInactiveDetails { get; set; }

    [XmlElement("hasInvItems")]
    public int HasInvItems { get; set; }
}