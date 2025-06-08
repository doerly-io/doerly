using Doerly.DataAccess.Models;

namespace Doerly.Module.Order.DataAccess.Entities;

public class OrderFeedback : BaseEntity
{
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public int ReviewerUserId { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; }
}
