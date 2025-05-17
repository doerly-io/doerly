using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class ProfileDto
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ESex Sex { get; set; }
    public string? Bio { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public string? ImageUrl { get; set; }
    public string? CvUrl { get; set; }
}
