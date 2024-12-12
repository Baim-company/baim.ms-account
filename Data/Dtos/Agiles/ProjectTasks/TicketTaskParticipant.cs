namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;
public record TicketTaskParticipant
{
    public Guid ProjectTiketUserId { get; set; }
    public string TaskRole { get; set; } = string.Empty;
}