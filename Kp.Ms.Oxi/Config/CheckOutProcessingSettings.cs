namespace Kp.Ms.Oxi.Config;

public class CheckOutProcessingSettings
{
    public MindboxSettings Mindbox { get; set; }
    public BookingSettings Booking { get; set; }
    public ShopSettings Shop { get; set; }
    public MailSettings Mail { get; set; }
    public string LogIndex { get; set; }
}
