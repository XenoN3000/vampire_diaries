namespace Domain;

public class Diary
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTim { get; set; }
    public DateTime Duration { get; set; }
    
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
}