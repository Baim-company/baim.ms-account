using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;
public class Client
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }


    public Guid CompanyId { get; set; }
    [JsonIgnore] 
    public Company? Company { get; set; }
    

    public User User { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public IEnumerable<ProjectUser>? ProjectUsers { get; set; }



    public Client()
    { 
        Id = Guid.NewGuid();
        IsActive = true;

        Company = null;
        CompanyId = Guid.Empty; 
    }
    public Client(User user)
    {
        Id = Guid.NewGuid();
        IsActive = true;
        
        UserId = user.Id;

        CompanyId = Guid.Empty;
    } 
    public Client(User user,Company company)
    {
        Id = Guid.NewGuid();
        IsActive = true;

        User = user;
        UserId = user.Id;

        Company = company; 
        CompanyId = company.Id; 
    }
}