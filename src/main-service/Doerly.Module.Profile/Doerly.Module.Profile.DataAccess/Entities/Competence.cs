namespace Doerly.Module.Profile.DataAccess.Entities;

public class Competence
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    
    public virtual Profile Profile { get; set; }

    public int CategoryId { get; set; }
    
    public string CategoryName { get; set; }
}
