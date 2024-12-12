namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;

public class SubTicketTaskParticipant
{
    public Guid ProjectSubTiketUserId { get; set; }
    public string TaskRole { get; set; } = string.Empty;
}