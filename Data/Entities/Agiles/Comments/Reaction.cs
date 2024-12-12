using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Comments;

public class Reaction
{
    public Guid Id { get; set; }
    public string Emoji { get; set; }

    [JsonIgnore]
    public Guid CommentId { get; set; }
    [JsonIgnore]
    public Comment Comment { get; set; }


    [JsonIgnore]
    public Guid UserId { get; set; } 
    public User User { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
}