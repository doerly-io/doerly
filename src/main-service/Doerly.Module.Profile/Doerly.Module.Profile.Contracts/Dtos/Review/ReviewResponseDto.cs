namespace Doerly.Module.Profile.Contracts.Dtos;

public class ReviewResponseDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ReviewerProfileResponseDto ReviewerProfile { get; set; }
    
}


public class ReviewerProfileResponseDto
{
    public int ProfileId { get; set; }

    public string FullName { get; set; }

    public string AvatarUrl { get; set; }
}
