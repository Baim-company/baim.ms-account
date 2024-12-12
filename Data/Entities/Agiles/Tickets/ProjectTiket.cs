using System.Text.Json.Serialization;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;

namespace PersonalAccount.API.Models.Entities.Agiles.Tickets;
public class ProjectTiket
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime Deadline { get; set; }


    public Guid ProjectId { get; set; }
    [JsonIgnore]
    public Project Project { get; set; }



    [JsonIgnore]
    public bool HasSubTiket { get; set; }

    public ICollection<ProjectTiketUser> ProjectTiketUsers { get; set; }


    public ICollection<ProjectTask> ProjectTasks { get; set; }
    public ICollection<ProjectSubTiket>? ProjectSubTikets { get; set; }



    public ProjectTiket()
    {
        Id = Guid.NewGuid();
        CreatedTime = DateTime.UtcNow.AddHours(4);
        Deadline = DateTime.UtcNow.AddHours(4).AddMonths(1);
    }
    public ProjectTiket(Guid projectId, string title)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;

        Title = title;
        CreatedTime = DateTime.UtcNow.AddHours(4);
        Deadline = DateTime.UtcNow.AddHours(4).AddMonths(1);

        HasSubTiket = false;
        ProjectSubTikets = new List<ProjectSubTiket>();
        if (ProjectSubTikets != null && ProjectSubTikets.Count() > 0) HasSubTiket = true;
    }
}