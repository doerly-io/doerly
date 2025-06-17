namespace Doerly.Module.Profile.DataTransferObjects;

public class ProfileShortInfoDto
{
    public required int Id { get; set; }

    public int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
