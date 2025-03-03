namespace Domain;

public class Task : BaseEntity
{
    public DateTime StartTim { get; set; }
    public DateTime Duration { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}