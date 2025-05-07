using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Options;

public class OpenSearchOptions
{
    public const string Position = "OpenSearch";

    [Required]
    public required string Url { get; set; }

    [Required]
    public required string User { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string LogIndex { get; set; }

    [Required]
    public required string LogIndexOperaMessages { get; set; }

    [Required]
    public required string LogIndexOperaRawMessages { get; set; }
}