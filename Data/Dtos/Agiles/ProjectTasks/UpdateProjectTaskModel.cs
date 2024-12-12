namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;

public record UpdateProjectTaskModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsVeryImportant { get; set; }

    public DateTime DeadLine { get; set; } = DateTime.UtcNow.AddMonths(1);
}