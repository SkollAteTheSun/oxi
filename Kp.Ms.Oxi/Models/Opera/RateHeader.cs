using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlRoot("RateHeader", Namespace = "rate.fidelio.6.0")]
public class RateHeader
{
    [XmlAttribute("rateStatus")]
    public string RateStatus { get; set; }

    [XmlAttribute("taxIncluded")]
    public string TaxIncluded { get; set; }

    [XmlAttribute("yieldStatus")]
    public string YieldStatus { get; set; }

    [XmlAttribute("sellSequence")]
    public int SellSequence { get; set; }

    [XmlAttribute("rateMessageType")]
    public string RateMessageType { get; set; }

    [XmlElement("hotelCode")]
    public string HotelCode { get; set; }

    [XmlElement("RateCategory")]
    public RateCategory RateCategory { get; set; }

    [XmlElement("rateCode")]
    public string RateCode { get; set; }

    [XmlElement("startSell")]
    public string StartSell { get; set; }

    [XmlElement("endSell")]
    public string EndSell { get; set; }

    [XmlElement("displayText")]
    public string DisplayText { get; set; }

    [XmlElement("folioText")]
    public string FolioText { get; set; }

    [XmlElement("longInfo")]
    public string LongInfo { get; set; }

    [XmlElement("shortInfo")]
    public string ShortInfo { get; set; }

    [XmlElement("minLos")]
    public int MinLengthOfStay { get; set; }

    [XmlElement("packages")]
    public List<string> Packages { get; set; }

    [XmlElement("marketCode")]
    public string MarketCode { get; set; }

    [XmlElement("sourceCode")]
    public string SourceCode { get; set; }

    [XmlElement("commission")]
    public int Commission { get; set; }

    [XmlElement("foreignCurrencyDepartmentCode")]
    public string ForeignCurrencyDepartmentCode { get; set; }

    [XmlElement("departmentCode")]
    public string DepartmentCode { get; set; }

    [XmlElement("confidential")]
    public string Confidential { get; set; }

    [XmlElement("printRate")]
    public int PrintRate { get; set; }

    [XmlElement("rateTiered")]
    public int RateTiered { get; set; }

    [XmlElement("negotiated")]
    public int Negotiated { get; set; }

    [XmlElement("dayUse")]
    public int DayUse { get; set; }

    [XmlElement("complimentary")]
    public int Complimentary { get; set; }

    [XmlElement("houseUse")]
    public int HouseUse { get; set; }

    [XmlElement("dayType")]
    public int DayType { get; set; }

    [XmlElement("discount")]
    public int Discount { get; set; }

    [XmlElement("packageTxnCode")]
    public int PackageTxnCode { get; set; }

    [XmlElement("RateDetails")]
    public RateDetails RateDetails { get; set; }

    [XmlElement("gdsAllowed")]
    public int GdsAllowed { get; set; }

    [XmlElement("RatePackages")]
    public RatePackages RatePackages { get; set; }

    [XmlElement("minOccupancy")]
    public int MinOccupancy { get; set; }

    [XmlElement("maxOccupancy")]
    public int MaxOccupancy { get; set; }

    [XmlElement("regionalAvailability")]
    public int RegionalAvailability { get; set; }

    [XmlElement("channelRestriction")]
    public int ChannelRestriction { get; set; }

    [XmlElement("channelRateUpdate")]
    public int ChannelRateUpdate { get; set; }

    [XmlElement("dailyRate")]
    public int DailyRate { get; set; }

    [XmlElement("rod")]
    public int Rod { get; set; }

    [XmlElement("rodBased")]
    public int RodBased { get; set; }

    [XmlElement("rodBaseFltPct")]
    public string RodBaseFltPct { get; set; }

    [XmlElement("rodBaseRounding")]
    public string RodBaseRounding { get; set; }

    [XmlElement("bbar")]
    public int Bbar { get; set; }

    [XmlElement("bbarBased")]
    public int BbarBased { get; set; }

    [XmlElement("bbarBaseFltPct")]
    public string BbarBaseFltPct { get; set; }

    [XmlElement("bbarBaseRounding")]
    public string BbarBaseRounding { get; set; }

    [XmlElement("dbaseFltPct")]
    public string DbaseFltPct { get; set; }

    [XmlElement("dbaseRounding")]
    public string DbaseRounding { get; set; }

    [XmlElement("dbaseCompare")]
    public int DbaseCompare { get; set; }

    [XmlElement("roomTypeList")]
    public List<string> RoomTypeList { get; set; }

    [XmlElement("mfnUpload")]
    public int MfnUpload { get; set; }

    [XmlElement("membership")]
    public int Membership { get; set; }

    [XmlElement("exchangePostingType")]
    public string ExchangePostingType { get; set; }

    [XmlElement("bbarCompare")]
    public int BbarCompare { get; set; }

    [XmlElement("fullDailyRateDetails")]
    public int FullDailyRateDetails { get; set; }

    [XmlElement("redemption")]
    public int Redemption { get; set; }

    [XmlElement("apartmentBillingCycle")]
    public string ApartmentBillingCycle { get; set; }

    [XmlElement("advBaseCompare")]
    public int AdvBaseCompare { get; set; }

    [XmlElement("defaultToHighestBar")]
    public int DefaultToHighestBar { get; set; }

    [XmlElement("repeatPostingRhythm")]
    public int RepeatPostingRhythm { get; set; }

    [XmlElement("discountRatePercentage")]
    public int DiscountRatePercentage { get; set; }
}