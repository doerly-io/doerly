namespace Doerly.Module.Authorization.DataTransferObjects.Responses;

public class UserItemResponse
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? RoleName { get; set; }
    public int? RoleId { get; set; }
}