namespace Doerly.Module.Profile.DataTransferObjects;

public class ProfileShortInfoWithAvatarDto : ProfileShortInfoDto
{
    public string FullName => $"{FirstName} {LastName}";
    
    public string? AvatarUrl { get; set; }
}
