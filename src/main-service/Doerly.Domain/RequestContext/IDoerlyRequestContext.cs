namespace Doerly.Domain;

public interface IDoerlyRequestContext
{
    int? UserId { get; set; }
    
    string? UserEmail { get; set; }
    
    string[]? UserRoles { get; set; }
    
}
