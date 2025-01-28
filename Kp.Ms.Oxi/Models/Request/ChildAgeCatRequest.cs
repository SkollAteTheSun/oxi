using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class ChildAgeCatRequest
{
    [Required]
    public required string Code { get; set; }

    [Required]
    public int AgeFrom { get; set; }

    [Required]
    public int AgeTo { get; set; }

    [Required]
    public int Count { get; set; }
}