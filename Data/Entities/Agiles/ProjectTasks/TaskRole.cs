using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
public class TaskRole
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [JsonIgnore]
    public Guid ProjectTaskId { get; set; }
    public ProjectTask ProjectTask { get; set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectUserRole ProjectUserRole { get; set; } = ProjectUserRole.Observer;

    public TaskRole()
    {
        Id = Guid.NewGuid();
    }
    public TaskRole(ProjectTask projectTask, User user, ProjectUserRole role)
    {
        ProjectTaskId = projectTask.Id;
        ProjectTask = projectTask;
        ProjectUserRole = role;
        UserId = user.Id;
        User = user;
    }
}