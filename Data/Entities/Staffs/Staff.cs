using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Staffs;
public class Staff
{
    public Guid Id { get; set; }
    public bool IsDismissed { get; set; }
    public ushort Experience { get; set; }

    public User User { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }


    public List<Certificate> Certificates { get; set; }
    public List<StaffImage> StaffImages { get; set; }
    [JsonIgnore]
    public IEnumerable<Project>? MyManageProjects { get; set; }
    [JsonIgnore]
    public IEnumerable<ProjectUser>? ProjectUsers { get; set; }


    public Staff() { }
    public Staff(User user)
    {
        Id = Guid.NewGuid();
        UserId = user.Id;
        User = user;
    }
    public Staff(StaffModel staffModel, User user)
    {
        Id = Guid.NewGuid(); 

        UserId = user.Id; 
        User = user;
        IsDismissed = staffModel.IsDismissed; 
        Experience = staffModel.Experience;
         
        Certificates = new List<Certificate>();
    }
}
