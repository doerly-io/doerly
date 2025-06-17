namespace Doerly.Module.Profile.DataAccess.Models;

public class Competence
{
    public int Id { get; set; }
    
    public int ProfileId { get; set; }
    
    public virtual Profile Profile { get; set; }
    
    public int CategoryId { get; set; }
    
    public string CategoryName { get; set; }
    
    public float? Rating { get; set; }

}
