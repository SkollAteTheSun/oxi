using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Options;

public class BookingApiOptions
{
    public const string Position = "BookingApi";
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Booking base url must not be empty.")]
    public string BaseUrl { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Booking token must not be empty.")]
    public string Token { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "booking transaction url must not be empty.")]
    public string TransactionGetResponseUrl { get; set; }
}