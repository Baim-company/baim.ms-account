using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Comments;

public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    [JsonIgnore]
    public Guid UserId { get; set; }

    public User? User { get; set; }

    [JsonIgnore]
    public ProjectTask? ProjectTask { get; set; }
    [JsonIgnore]
    public Guid ProjectTaskId { get; set; }

    public List<CommentFile>? Files { get; set; }
    public List<Reaction>? Reactions { get; set; } 

    public List<Mention>? Mentions { get; set; } 

    public Comment()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow.AddHours(4);
    }
}