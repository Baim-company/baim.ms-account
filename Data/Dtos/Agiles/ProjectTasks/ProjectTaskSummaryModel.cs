using PersonalAccount.API.Models.Dtos.Agiles.CheckItems;
using PersonalAccount.API.Models.Dtos.Agiles.Comments;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks
{
    public class ProjectTaskSummaryModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVeryImportant { get; set; }
        public bool IsExpired { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DoneState DoneState { get; set; } = DoneState.InProcess;
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public ProjectSummaryModel Project { get; set; }
        public ProjectTiketSummaryModel? ProjectTicket { get; set; }
        public ProjectSubTicketSummaryModel? ProjectSubTicket { get; set; }
        public ICollection<ProjectTaskCheckItemModel> ProjectTaskCheckItems { get; set; }
        public ICollection<CommentSummaryModel> Comments { get; set; }
        public ICollection<ProjectTicketTaskUserSummaryModel>? ProjectTicketTaskUsers { get; set; }
        public ICollection<ProjectSubTicketUserSummaryModel>? ProjectSubTicketTaskUsers { get; set; }


    }
}
