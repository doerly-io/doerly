using Doerly.DataAccess.Models;

namespace Doerly.Module.Profile.DataAccess.Entities;

public class Review : BaseEntity
{
    public int Rating { get; set; }

    public string Comment { get; set; }

    public int ReviewerUserId { get; set; }

    public int ProfileId { get; set; }

    public virtual Profile Profile { get; set; }

    public virtual Profile ReviewerProfile { get; set; }
}
