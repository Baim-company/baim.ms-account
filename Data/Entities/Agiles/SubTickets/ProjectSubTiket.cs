using System.Text.Json.Serialization;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;

namespace PersonalAccount.API.Models.Entities.Agiles.SubTickets;
public class ProjectSubTiket
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime Deadline { get; set; }

    [JsonIgnore]
    public Guid ProjectTiketId { get; set; }
    [JsonIgnore]
    public ProjectTiket ProjectTiket { get; set; }


    // Участники 0.25 
    public List<ProjectSubTiketUser> ProjectSubTiketUsers { get; set; }


    public List<ProjectTask>? ProjectTasks { get; set; }

    public ProjectSubTiket()
    {
        Id = Guid.NewGuid();
        CreatedTime = DateTime.UtcNow.AddHours(4);
        Deadline = DateTime.UtcNow.AddHours(4).AddMonths(1);
    }
    public ProjectSubTiket(Guid projectTiketId, string title)
    {
        Id = Guid.NewGuid();
        CreatedTime = DateTime.UtcNow.AddHours(4);
        Deadline = DateTime.UtcNow.AddHours(4).AddMonths(1);

        ProjectTiketId = projectTiketId;
        Title = title;
    }
}