using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Projects;
public class ProjectUser
{
    public Guid Id { get; set; }


    [JsonIgnore]
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }

    [JsonIgnore]
    public Guid? StaffId { get; set; }
    public Staff? Staff { get; set; }


    [JsonIgnore]
    public Guid ProjectId { get; set; }
    [JsonIgnore]
    public Project? Project { get; set; }


    public ProjectUser()
    {
        Id = Guid.NewGuid();
    }
}