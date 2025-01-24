using System.ComponentModel.DataAnnotations;

namespace Doerly.DataAccess.Models;

public class BaseEntity : IBaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime DateCreated { get; set; }

    public DateTime LastModifiedDate { get; set; }

}
