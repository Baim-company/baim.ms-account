namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;

public record UpdateProjectSubTiketModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }

    public Guid ProjectSubTicketId { get; set; }

    public List<Guid>? projectTicketUserIds { get; set; }
}