using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Users;
public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    [JsonIgnore]
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public Role() 
    { 
        Id = Guid.NewGuid();
    }
    public Role(string role)
    {
        Id = Guid.NewGuid();
        RoleName = role;
    }
    public Role(Guid id, string role)
    {
        Id = id;
        RoleName = role;
    }
}