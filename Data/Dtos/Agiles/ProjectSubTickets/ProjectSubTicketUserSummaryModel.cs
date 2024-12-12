using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets
{
    public class ProjectSubTicketUserSummaryModel
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectUserRole ProjectUserRole { get; set; } = ProjectUserRole.Executor;

        [JsonIgnore]
        public Guid ProjectSubTiketUserId { get; set; }
        public ProjectSubTiketUser ProjectSubTiketUser { get; set; }

    }
}
