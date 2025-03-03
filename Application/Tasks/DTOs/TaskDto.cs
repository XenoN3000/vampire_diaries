using Domain;

namespace Application.Tasks.DTOs;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public DateTime Duration { get; set; }
    public Guid ProjectId { get; set; }
}