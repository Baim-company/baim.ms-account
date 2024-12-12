using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
public class ProjectSubTicketTaskUser
{
    public Guid Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectUserRole ProjectUserRole { get; set; } = ProjectUserRole.Executor;



    [JsonIgnore]
    public Guid ProjectSubTiketUserId { get; set; }
    public ProjectSubTiketUser ProjectSubTiketUser { get; set; }

    [JsonIgnore]
    public Guid ProjectTaskId { get; set; }
    [JsonIgnore]
    public ProjectTask ProjectTask { get; set; } 



    public ProjectSubTicketTaskUser()
    {
        Id = Guid.NewGuid();
    }
    public ProjectSubTicketTaskUser(ProjectUserRole projectUserRole)
    {
        Id = Guid.NewGuid();
        ProjectUserRole = projectUserRole;
    }
}