using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.CheckLists;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsVeryImportant { get; set; }
    public bool IsExpired { get; private set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DoneState DoneState { get; set; } = DoneState.InProcess;


    public DateTime StartDate { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime DeadLine { get; set; }


    [JsonIgnore]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }



    [JsonIgnore]
    public Guid? ProjectTicketId { get; set; }
    [JsonIgnore]
    public ProjectTiket? ProjectTicket { get; set; }



    [JsonIgnore]
    public Guid? ProjectSubTicketId { get; set; }
    [JsonIgnore]
    public ProjectSubTiket? ProjectSubTicket { get; set; }

    [JsonIgnore]
    public Guid? ProjectTaskCheckListId { get; set; }
    public ProjectTaskCheckList? ProjectTaskCheckList { get; set; }
    public ICollection<Comment> Comments { get; set; }



    public ICollection<ProjectTicketTaskUser>? ProjectTicketTaskUsers { get; set; }
    public ICollection<ProjectSubTicketTaskUser>? ProjectSubTicketTaskUsers { get; set; }




    public ProjectTask()
    {
        Id = Guid.NewGuid();
    }
    public ProjectTask(string title, string desc, bool isVeryImportant, DateTime deadline)
    {
        Title = title;
        Description = desc;
        DeadLine = deadline;
        IsVeryImportant = isVeryImportant;
        StartDate = DateTime.UtcNow.AddHours(4);
        DeadLine = deadline;
    }
}