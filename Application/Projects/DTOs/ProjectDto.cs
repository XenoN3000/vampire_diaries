using System.Text.Json.Serialization;
using Domain;
using UserTask = Domain.Task;

namespace Application.Projects.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }
    public DateTime CreateAt { get; set; }

    public string OwnerId { get; set; }
    [JsonIgnore] public AppUser Owner { get; set; }
    
    public ICollection<UserTask> Tasks { get; set; }
}