namespace Doerly.Module.Order.DataTransferObjects;
public class ProfileInfo
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? AvatarUrl { get; set; }
}
