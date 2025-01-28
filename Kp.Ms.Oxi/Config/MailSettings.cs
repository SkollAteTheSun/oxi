namespace Kp.Ms.Oxi.Config;

public class MailSettings
{
    public string Url { get; set; }
    public List<string> Emails { get; set; }

    public MailSettings()
    {
        Emails = new List<string>();
    }
}
