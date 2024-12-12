using System.Text.Json.Serialization;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;

namespace PersonalAccount.API.Models.Entities.Agiles.Tickets;
public class ProjectTiketUser
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid ProjectUserId { get; set; }
    public ProjectUser ProjectUser { get; set; }


    [JsonIgnore]
    public Guid ProjectTiketId { get; set; }
    [JsonIgnore]
    public ProjectTiket ProjectTiket { get; set; }


    [JsonIgnore]
    public ICollection<ProjectTicketTaskUser> ProjectTicketTaskUsers { get; set; }
    public ProjectTiketUser()
    {
        Id = Guid.NewGuid();
    }
}