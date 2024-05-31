using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Response;

public class TransactionDto
{
    [Required]
    public required string Url { get; set; }

    [Required]
    public required string Xml { get; set; }
}