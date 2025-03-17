namespace Domain;

public class Task : BaseEntity
{
    public DateTime StartTim { get; set; }
    public string Duration { get; set; }
    
    public string OwnerId { get; set; }
    
    public AppUser Owner { get; set; }
}