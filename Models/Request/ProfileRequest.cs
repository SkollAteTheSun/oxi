using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class ProfileRequest
{
    [Required]
    public ulong Id { get; set; }
    
    public string? ExternalId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string SurName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    public required string Phone { get; set; }

    [Required]
    public required string Email { get; set; }
}