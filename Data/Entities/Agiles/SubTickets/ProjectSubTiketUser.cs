using System.Text.Json.Serialization;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;

namespace PersonalAccount.API.Models.Entities.Agiles.SubTickets;
public class ProjectSubTiketUser
{
    public Guid Id { get; set; }


    [JsonIgnore]
    public Guid ProjectTiketUserId { get; set; }
    public ProjectTiketUser? ProjectTiketUser { get; set; }


    [JsonIgnore]
    public Guid ProjectSubTiketId { get; set; }
    [JsonIgnore]
    public ProjectSubTiket? ProjectSubTiket { get; set; }


    public ICollection<ProjectTask>? ProjectTasks { get; set; }
    [JsonIgnore]
    public ICollection<ProjectSubTicketTaskUser>? ProjectSubTicketTaskUsers { get; set; }


    public ProjectSubTiketUser()
    {
        Id = Guid.NewGuid();
    }
}