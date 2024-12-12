namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;

public class ProjectSubTicketTaskModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsVeryImportant { get; set; }

    public DateTime DeadLine { get; set; } = DateTime.UtcNow.AddMonths(1);


    public Guid ProjectId { get; set; }
    public Guid ProjectTicketId { get; set; }
    public Guid ProjectSubTicketId { get; set; }

    public List<SubTicketTaskParticipant>? SubTicketTaskParticipants { get; set; }
}