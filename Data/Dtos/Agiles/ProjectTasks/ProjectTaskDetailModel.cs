using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Enums;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;

public class ProjectTaskModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsVeryImportant { get; set; }
    public bool IsExpired { get; set; }
    public DoneState DoneState { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DeadLine { get; set; }
    public Project Project { get; set; }
    public ProjectTiket ProjectTicket { get; set; }
    public ProjectSubTiket ProjectSubTicket { get; set; }
    public IEnumerable<ProjectTaskCheckItem> ProjectTaskCheckItems { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
    public IEnumerable<ProjectTicketTaskUser> ProjectTicketTaskUsers { get; set; }
    public IEnumerable<ProjectSubTicketTaskUser> ProjectSubTicketTaskUsers { get; set; }
}