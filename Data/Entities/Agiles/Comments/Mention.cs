using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Comments;

public class Mention
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [JsonIgnore]
    public Guid CommentId { get; set; }
    [JsonIgnore]
    public Comment Comment { get; set; }

    public Mention()
    {
        Id = Guid.NewGuid();
    }
}
