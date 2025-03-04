using Domain;

namespace Application.Projects.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public string Description { get; set; }
    public DateTime CreateAt { get; set; }

    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    
}