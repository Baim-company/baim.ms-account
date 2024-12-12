using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets
{
    public class ProjectSubTicketSummaryModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<ProjectTiketUser> ProjectTiketUsers { get; set; }
        public ICollection<ProjectTaskCheckItem> ProjectTaskCheckItems { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<ProjectTicketTaskUser>? ProjectTicketTaskUsers { get; set; }
        public ICollection<ProjectSubTicketTaskUser>? ProjectSubTicketTaskUsers { get; set; }

    }
}
