namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
public record UpdateProjectTiketModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }

    public Guid ProjectId { get; set; }

    public List<Guid>? projectUserIds { get; set; }
}