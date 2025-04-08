using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Options;

public class ReservationTaxOptions
{
    public const string Position = "ReservationTaxDefaults";

    [Required(AllowEmptyStrings = false, ErrorMessage = "TouristTaxMapping must not be empty.")]
    public string TouristTaxMapping { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "TouristTaxValue must not be empty.")]
    public string TouristTaxValue { get; set; }
}
