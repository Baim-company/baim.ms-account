using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;

public class ProjectTicketTaskUser
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid ProjectTiketUserId { get; set; }  
    public ProjectTiketUser ProjectTiketUser { get; set; }



    [JsonIgnore]
    public Guid ProjectTaskId { get; set; }
    [JsonIgnore]
    public ProjectTask ProjectTask { get; set; }



    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectUserRole ProjectUserRole { get; set; } = ProjectUserRole.Executor;


    public ProjectTicketTaskUser()
    {
        Id = Guid.NewGuid();
    }
    public ProjectTicketTaskUser(ProjectUserRole projectUserRole)
    {
        Id = Guid.NewGuid();
        ProjectUserRole = projectUserRole;
    }
}