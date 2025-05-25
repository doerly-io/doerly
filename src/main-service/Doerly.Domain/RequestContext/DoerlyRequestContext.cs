namespace Doerly.Domain;

public class DoerlyRequestContext : IDoerlyRequestContext
{
    public int? UserId { get; set; }
    
    public string? UserEmail { get; set; }
    
    public string[]? UserRoles { get; set; }
}
