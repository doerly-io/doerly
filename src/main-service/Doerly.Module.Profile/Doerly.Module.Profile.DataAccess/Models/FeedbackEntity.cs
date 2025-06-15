using Doerly.DataAccess.Models;

namespace Doerly.Module.Profile.DataAccess.Models;

public class FeedbackEntity : BaseEntity
{
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public int ReviewerUserId { get; set; }

    public int RevieweeUserId { get; set; }
    
    public int CategoryId { get; set; }

    public int OrderId { get; set; }

    public Profile ReviewerProfile { get; set; }

    public Profile RevieweeProfile { get; set; }
}