using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Users;
public class UserRole
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public UserRole() { }
    public UserRole(User user, Role role)
    {
        User = user;
        UserId = user.Id;
        Role = role;
        RoleId = role.Id;
    }
}