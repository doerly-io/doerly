using Doerly.DataAccess.Models;

namespace Doerly.Module.Profile.DataAccess.Entities;

public class Review : BaseEntity
{
    public int Rating { get; set; }
    
    public string Comment { get; set; }
    
    public int ReviewerId { get; set; }
    
    public int RevieweeId { get; set; }

    public virtual Profile Reviewer { get; set; }
    public virtual Profile Reviewee { get; set; }
}
