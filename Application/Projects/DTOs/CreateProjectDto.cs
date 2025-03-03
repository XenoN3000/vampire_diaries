namespace Application.Projects.DTOs;

public class CreateProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public string Description { get; set; }
    
}