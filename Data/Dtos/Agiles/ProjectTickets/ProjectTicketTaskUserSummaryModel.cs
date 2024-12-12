using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets
{
    public class ProjectTicketTaskUserSummaryModel
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProjectTiketUserId { get; set; }
        public ProjectTiketUser ProjectTiketUser { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectUserRole ProjectUserRole { get; set; } = ProjectUserRole.Executor;

    }
}
