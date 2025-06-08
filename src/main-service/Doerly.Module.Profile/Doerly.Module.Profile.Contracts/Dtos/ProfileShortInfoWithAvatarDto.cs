namespace Doerly.Module.Profile.Contracts.Dtos;

public class ProfileShortInfoWithAvatarDto : ProfileShortInfoDto
{
    public string FullName => $"{FirstName} {LastName}";
    
    public string? AvatarUrl { get; set; }
}
