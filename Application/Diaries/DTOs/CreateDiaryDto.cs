namespace Application.Diaries.DTOs;

public class CreateDiaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public DateTime Duration { get; set; }
}