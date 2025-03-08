namespace Domain;

public class Task : BaseEntity
{
    public DateTime StartTim { get; set; }
    public DateTime Duration { get; set; }
    
    public string OwnerId { get; set; }
    
    public AppUser Owner { get; set; }
}