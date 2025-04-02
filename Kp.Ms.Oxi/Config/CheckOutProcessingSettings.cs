namespace Kp.Ms.Oxi.Config;

public class CheckOutProcessingSettings
{
    public MindboxSettings Mindbox { get; set; }
    public BookingSettings Booking { get; set; }
    public ShopSettings ShopEditReservationStatus { get; set; }
    public ShopSettings ShopFullPaymentReceipt { get; set; }
    public MailSettings Mail { get; set; }
    public List<string> IgnoreHotels { get; set; } = [];
    public string LogIndex { get; set; }
}
