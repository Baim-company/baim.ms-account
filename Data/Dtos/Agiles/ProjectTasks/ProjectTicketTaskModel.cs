namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;
public record ProjectTicketTaskModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsVeryImportant { get; set; }

    public DateTime DeadLine { get; set; } = DateTime.UtcNow.AddMonths(1);


    public Guid ProjectId { get; set; }
    public Guid ProjectTicketId { get; set; }

    public List<TicketTaskParticipant>? TicketTaskParticipants { get; set; }
}