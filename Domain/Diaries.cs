namespace Domain;

public class Diaries
{

    public Guid Id { get; set; }
    public string Data { get; set; }
    public DateTime StartTim { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime Duration { get; set; }

    public AppUser Owner { get; set; }
    
}