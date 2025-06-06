namespace Doerly.Module.Profile.Contracts.Dtos;

public class UserInfo
{
    public required int UserId { get; set; }
    public required string Email { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? RoleName { get; set; }
    public int? RoleId { get; set; }
}