using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class TransactionOneRequest
{
    [Required]
    public required string PropertyName { get; set; }

    // [Required]
    // public required string Login { get; set; }
    //
    // [Required]
    // public required string Password { get; set; }
}