using PersonalAccount.API.Models.Entities.Agiles.SubTickets;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;

public class ProjectTiketSummaryModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime Deadline { get; set; }
    public bool HasSubTiket { get; set; }
    public List<ProjectSubTiketUser> ProjectSubTiketUsers { get; set; }

}