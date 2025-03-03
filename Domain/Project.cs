using UserTask = Domain.Task;

namespace Domain;

public class Project: BaseEntity
{
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public DateTime CreateAt { get; set; }
    public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
}