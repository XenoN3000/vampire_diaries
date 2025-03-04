using System.Text.Json.Serialization;

namespace Application.Projects.DTOs;

public class CreateProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    
    [JsonIgnore]
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    
    
}