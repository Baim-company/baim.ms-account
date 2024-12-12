namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;

public record ProjectSubTiketModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }

    public Guid ProjectTicketId { get; set; }

    public List<Guid>? projectTicketUserIds { get; set; }
}